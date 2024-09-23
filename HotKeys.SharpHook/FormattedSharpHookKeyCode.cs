using CommunityToolkit.Diagnostics;
using SharpHook.Native;

namespace HotKeys.SharpHook;

public readonly struct FormattedSharpHookKeyCode : IEquatable<FormattedSharpHookKeyCode>, IComparable
{
	public static implicit operator FormattedSharpHookKeyCode(KeyCode keyCode)
	{
		return new FormattedSharpHookKeyCode(keyCode);
	}

	public static implicit operator KeyCode(FormattedSharpHookKeyCode formatted)
	{
		return formatted.KeyCode;
	}

	public static bool operator ==(FormattedSharpHookKeyCode left, FormattedSharpHookKeyCode right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FormattedSharpHookKeyCode left, FormattedSharpHookKeyCode right)
	{
		return !(left == right);
	}

	public KeyCode KeyCode { get; }

	public FormattedSharpHookKeyCode(KeyCode keyCode)
	{
		KeyCode = keyCode;
	}

	public bool Equals(FormattedSharpHookKeyCode other)
	{
		return KeyCode == other.KeyCode;
	}

	public int CompareTo(object? other)
	{
		Guard.IsNotNull(other);
		return SharpHookComparingHelper.Compare(this, other);
	}

	public override bool Equals(object? obj)
	{
		return obj is FormattedSharpHookKeyCode other && Equals(other);
	}

	public override int GetHashCode()
	{
		return (int)KeyCode;
	}

	public override string ToString()
	{
		const string keyCodePrefix = "Vc";
		var keyCodeName = KeyCode.ToString();
		Guard.IsTrue(keyCodeName.StartsWith(keyCodePrefix));
		return keyCodeName[keyCodePrefix.Length..];
	}
}