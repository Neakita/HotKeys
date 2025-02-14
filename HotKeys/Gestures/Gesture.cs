using System.Collections.Immutable;

namespace HotKeys.Gestures;

public sealed class Gesture
{
	public static Gesture Empty { get; } = new();

	public ImmutableHashSet<object> Keys { get; }
	public bool IsEmpty => Keys.IsEmpty;

	public Gesture(params ImmutableHashSet<object> keys)
	{
		Keys = keys;
	}

	public override string ToString()
	{
		return string.Join(" + ", Keys);
	}

	public override bool Equals(object? obj)
	{
		return ReferenceEquals(this, obj) || obj is Gesture other && Equals(other);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = new();
		foreach (object key in Keys)
			hashCode.Add(key);
		return hashCode.ToHashCode();
	}

	private bool Equals(Gesture other)
	{
		return Keys.SetEquals(other.Keys);
	}
}