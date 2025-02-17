namespace HotKeys;

public interface InputState<out TKey>
{
	TKey Key { get; }
	bool IsPressed { get; }
}