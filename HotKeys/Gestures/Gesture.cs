namespace HotKeys.Gestures;

public abstract class Gesture
{
	public abstract IReadOnlySet<object> Keys { get; }

	public override string ToString()
	{
		return string.Join(" + ", Keys);
	}
}