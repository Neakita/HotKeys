using CommunityToolkit.Diagnostics;
using SharpHook.Native;

namespace Katter.HotKeys.SharpHook;

public record KeyGesture(KeyCode Key, KeyModifiers Modifiers)
{
	private const string KeyCodePrefix = "Vc";

	public override string ToString()
	{
		var key = Key.ToString();
		Guard.IsTrue(key.StartsWith(KeyCodePrefix));
		key = key[KeyCodePrefix.Length..];
		if (Modifiers == KeyModifiers.None)
			return key;
		var modifiers = Modifiers.ToString();
		modifiers = string.Join(" + ", modifiers.Split(", "));
		return $"{modifiers} + {key}";
	}
}