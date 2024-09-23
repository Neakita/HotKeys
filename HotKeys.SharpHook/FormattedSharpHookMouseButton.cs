using CommunityToolkit.Diagnostics;
using SharpHook.Native;

namespace HotKeys.SharpHook;

public readonly struct FormattedSharpHookMouseButton : IEquatable<FormattedSharpHookMouseButton>, IComparable
{
	public static implicit operator FormattedSharpHookMouseButton(MouseButton button)
	{
		return new FormattedSharpHookMouseButton(button);
	}

	public static implicit operator MouseButton(FormattedSharpHookMouseButton formatted)
	{
		return formatted.Button;
	}

	public MouseButton Button { get; }

	public FormattedSharpHookMouseButton(MouseButton button)
	{
		Button = button;
	}

	public bool Equals(FormattedSharpHookMouseButton other)
	{
		return Button == other.Button;
	}

	public int CompareTo(object? other)
	{
		Guard.IsNotNull(other);
		return SharpHookComparingHelper.Compare(this, other);
	}

	public override bool Equals(object? obj)
	{
		return obj is FormattedSharpHookMouseButton other && Equals(other);
	}

	public override int GetHashCode()
	{
		return (int)Button;
	}

	public override string ToString() => Button switch
	{
		MouseButton.Button1 => "Left Mouse Button",
		MouseButton.Button2 => "Right Mouse Button",
		MouseButton.Button3 => "Middle Mouse Button",
		MouseButton.Button4 => "Side Back Mouse Button",
		MouseButton.Button5 => "Side Front Mouse Button",
		_ => throw new ArgumentOutOfRangeException()
	};
}