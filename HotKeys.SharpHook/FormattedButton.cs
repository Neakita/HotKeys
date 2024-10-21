using CommunityToolkit.Diagnostics;
using SharpHook.Native;

namespace HotKeys.SharpHook;

public readonly struct FormattedButton : IEquatable<FormattedButton>, IComparable
{
	public static implicit operator FormattedButton(MouseButton button)
	{
		return new FormattedButton(button);
	}

	public static implicit operator MouseButton(FormattedButton formatted)
	{
		return formatted.Button;
	}

	public static bool operator ==(FormattedButton left, FormattedButton right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FormattedButton left, FormattedButton right)
	{
		return !(left == right);
	}

	public MouseButton Button { get; }

	public FormattedButton(MouseButton button)
	{
		Button = button;
	}

	public bool Equals(FormattedButton other)
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
		return obj is FormattedButton other && Equals(other);
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