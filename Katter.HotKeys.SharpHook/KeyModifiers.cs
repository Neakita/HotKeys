namespace Katter.HotKeys.SharpHook;

[Flags]
public enum KeyModifiers
{
	None,
	Alt = 0b1,
	Control = 0b10,
	Shift = 0b100,
	Meta = 0b1000,
}