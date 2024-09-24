using System.Collections.Immutable;

namespace HotKeys.Gestures;

public sealed class Gesture
{
	public static Gesture Empty { get; } = new(ImmutableSortedSet<object>.Empty);

	public ImmutableSortedSet<object> Keys { get; }

	public Gesture(ImmutableSortedSet<object> keys)
	{
		Keys = keys;
	}

	public override string ToString()
	{
		return string.Join(" + ", Keys);
	}
}