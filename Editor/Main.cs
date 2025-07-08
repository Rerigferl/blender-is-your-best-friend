using System;
using System.IO;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Numeira;

internal static class Blender
{
    public static void Launch(string openFilePath)
    {
        if (string.IsNullOrEmpty(Preference.instance.BlenderPath))
        {
            if (!Menu.SelectBlender())
                return;
        }

        var pythonCode = @$"import bpy
bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)

bpy.ops.import_scene.fbx(filepath='{openFilePath}')
";
        Debug.LogError(pythonCode.Replace("\n\n", "\n").Replace("\n", "; "));
        using var process = Process.Start(Preference.instance.BlenderPath, @$"--python-expr ""{pythonCode.Replace("\n\n", "\n").Replace("\n", "; ")}""");
    }
}

internal static class Menu
{
    private const string MenuPath = "Assets/Open in Blender...";
    private const int MenuPriority = int.MaxValue;

    [MenuItem(MenuPath, false, MenuPriority)]
    public static void Open()
    {
        Blender.Launch(AssetDatabase.GetAssetPath(Selection.activeObject));
    }

    [MenuItem(MenuPath, true, MenuPriority)]
    public static bool OpenValidate()
    {
        var obj = Selection.activeObject;
        if (AssetDatabase.GetAssetPath(obj) is { } path && Path.GetExtension(path.AsSpan()).Equals(".fbx", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    [MenuItem("Tools/Numeira/Blender/Select Blender location...")]
    public static bool SelectBlender()
    {
        var path = EditorUtility.OpenFilePanel("Select Blender Execution File", @"C:\Program Files\Blender Foundation\", "exe");
        if (string.IsNullOrEmpty(path))
            return false;
        Preference.instance.BlenderPath = path;
        Preference.Save();
        return true;
    }
}
