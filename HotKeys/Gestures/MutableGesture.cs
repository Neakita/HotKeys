namespace HotKeys.Gestures;

internal sealed class MutableGesture : Gesture
{
	public override SortedSet<object> Keys { get; } = new();
}