using CommunityToolkit.Diagnostics;
using SharpHook.Native;

namespace HotKeys.SharpHook;

public readonly struct FormattedSharpButton : IEquatable<FormattedSharpButton>, IComparable
{
	public static implicit operator FormattedSharpButton(MouseButton button)
	{
		return new FormattedSharpButton(button);
	}

	public static implicit operator MouseButton(FormattedSharpButton formatted)
	{
		return formatted.Button;
	}

	public static bool operator ==(FormattedSharpButton left, FormattedSharpButton right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FormattedSharpButton left, FormattedSharpButton right)
	{
		return !(left == right);
	}

	public MouseButton Button { get; }

	public FormattedSharpButton(MouseButton button)
	{
		Button = button;
	}

	public bool Equals(FormattedSharpButton other)
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
		return obj is FormattedSharpButton other && Equals(other);
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