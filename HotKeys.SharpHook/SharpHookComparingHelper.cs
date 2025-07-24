using System.Collections.Frozen;
using SharpHook.Data;

namespace HotKeys.SharpHook;

internal static class SharpHookComparingHelper
{
	private static readonly FrozenSet<KeyCode> ModifierKeys = FrozenSet.ToFrozenSet(
	[
		KeyCode.VcLeftShift,
		KeyCode.VcRightShift,
		KeyCode.VcLeftAlt,
		KeyCode.VcRightAlt,
		KeyCode.VcLeftControl,
		KeyCode.VcRightControl,
		KeyCode.VcLeftMeta,
		KeyCode.VcRightMeta
	]);

	public static int Compare(object first, object second)
	{
		var firstPriority = GetPriority(first);
		var secondPriority = GetPriority(second);
		return firstPriority.CompareTo(secondPriority);
	}

	private static uint GetPriority(object value) => value switch
	{
		FormattedKeyCode keyCode => GetKeyPriority(keyCode),
		FormattedButton mouseButton => GetMouseButtonPriority(mouseButton),
		_ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
	};

	private static uint GetKeyPriority(KeyCode keyCode)
	{
		bool isModifier = ModifierKeys.Contains(keyCode);
		if (isModifier)
			return (uint)keyCode;
		return (uint)keyCode + ushort.MaxValue;
	}

	private static uint GetMouseButtonPriority(MouseButton button)
	{
		return (uint)button + ushort.MaxValue * 2;
	}
}