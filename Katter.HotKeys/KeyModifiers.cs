namespace Katter.HotKeys;

[Flags]
public enum KeyModifiers : byte
{
	None,
	LeftAlt = 1 << 0,
	RightAlt = 1 << 1,
	LeftControl = 1 << 2,
	RightControl = 1 << 3,
	LeftShift = 1 << 4,
	RightShift = 1 << 5,
	LeftMeta = 1 << 6,
	RightMeta = 1 << 7,
	Alt = LeftAlt | RightAlt,
	Control = LeftControl | RightControl,
	Shift = LeftShift | RightShift,
	Meta = LeftMeta | RightMeta,
	All = Alt | Control | Shift | Meta
}