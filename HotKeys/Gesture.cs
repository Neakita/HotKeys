using System.Collections.Immutable;

namespace HotKeys;

public sealed class Gesture
{
	public static Gesture Empty { get; } = new();

	public ImmutableHashSet<object> Keys { get; }
	public int Length => Keys.Count;
	public bool IsEmpty => Keys.IsEmpty;

	public Gesture(params ImmutableHashSet<object> keys)
	{
		Keys = keys;
	}

	public Gesture With(object key)
	{
		return new Gesture(Keys.Add(key));
	}

	public Gesture Without(object key)
	{
		return new Gesture(Keys.Remove(key));
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