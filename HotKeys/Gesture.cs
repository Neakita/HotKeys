using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

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
		var builder = Keys.ToBuilder();
		bool isAdded = builder.Add(key);
		Guard.IsTrue(isAdded);
		var keys = builder.ToImmutable();
		return new Gesture(keys);
	}

	public Gesture Without(object key)
	{
		var builder = Keys.ToBuilder();
		bool isRemoved = builder.Remove(key);
		Guard.IsTrue(isRemoved);
		var keys = builder.ToImmutable();
		return new Gesture(keys);
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