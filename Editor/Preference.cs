using System;
using System.IO;
using System.Reflection;
using UnityEditor;

namespace Numeira;

[Serializable]
[FilePath("numeira/blender-is-your-best-friend.json", FilePathAttribute.Location.PreferencesFolder)]
internal sealed class Preference
{
    public static Preference Instance => instance ??= new();
    private static Preference? instance;


    public string? BlenderPath;

    private Preference()
    {
        if (FilePath is not { } filePath || !File.Exists(filePath))
            return;

        var json = File.ReadAllText(filePath);

        EditorJsonUtility.FromJsonOverwrite(json, this);
    }

    public static void Save()
    {
        if (FilePath is not { } filePath)
            return;

        var json = EditorJsonUtility.ToJson(Instance, true);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        File.WriteAllText(filePath, json);
    }

    private static string? FilePath => typeof(FilePathAttribute).GetProperty("filepath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(typeof(Preference).GetCustomAttribute<FilePathAttribute>()) as string;
}