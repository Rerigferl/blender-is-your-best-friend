using UnityEditor;

namespace Numeira;

[FilePath("numeira/blender-is-your-best-friend.asset", FilePathAttribute.Location.PreferencesFolder)]
internal sealed class Preference : ScriptableSingleton<Preference>
{
    public string? BlenderPath;

    internal static void Save() => instance.Save(true);
}