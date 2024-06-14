using System.Diagnostics;
using SharpHook.Native;

namespace Katter.HotKeys.SharpHook;

public record KeyGesture(KeyCode Key, KeyModifiers Modifiers)
{
	public override string ToString()
	{
		var key = Key.ToString();
		if (key[..2] == "Vc")
			key = key[2..];
		else Debug.WriteLine($"{Key} expected to begin with \"Vc\" prefix");
		if (Modifiers == KeyModifiers.None)
			return key;
		var modifiers = Modifiers.ToString();
		modifiers = string.Join(" + ", modifiers.Split(", "));
		return $"{modifiers} + {key}";
	}
}