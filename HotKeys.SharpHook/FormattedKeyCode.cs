using CommunityToolkit.Diagnostics;
using SharpHook.Data;

namespace HotKeys.SharpHook;

public readonly struct FormattedKeyCode : IEquatable<FormattedKeyCode>, IComparable
{
	public static implicit operator FormattedKeyCode(KeyCode keyCode)
	{
		return new FormattedKeyCode(keyCode);
	}

	public static implicit operator KeyCode(FormattedKeyCode formatted)
	{
		return formatted.KeyCode;
	}

	public static bool operator ==(FormattedKeyCode left, FormattedKeyCode right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FormattedKeyCode left, FormattedKeyCode right)
	{
		return !(left == right);
	}

	public KeyCode KeyCode { get; }

	public FormattedKeyCode(KeyCode keyCode)
	{
		KeyCode = keyCode;
	}

	public bool Equals(FormattedKeyCode other)
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
		return obj is FormattedKeyCode other && Equals(other);
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