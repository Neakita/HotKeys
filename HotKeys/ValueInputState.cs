namespace HotKeys;

public readonly struct ValueInputState<TKey> : InputState<TKey> where TKey : notnull
{
	public TKey Key { get; }
	public bool IsPressed { get; }

	public ValueInputState(TKey key, bool isPressed)
	{
		Key = key;
		IsPressed = isPressed;
	}
}