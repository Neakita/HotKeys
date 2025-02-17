namespace HotKeys;

public sealed class ImmutableInputState<T> : InputState<T>
	where T : notnull
{
	public T Key { get; }
	public bool IsPressed { get; }

	public ImmutableInputState(T key, bool isPressed)
	{
		Key = key;
		IsPressed = isPressed;
	}
}