namespace HotKeys.Gestures;

internal sealed class MutableGesture : Gesture
{
	public override HashSet<object> Keys { get; } = new();
}