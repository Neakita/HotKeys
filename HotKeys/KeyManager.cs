namespace HotKeys;

public interface KeyManager
{
	IObservable<object> KeyPressed { get; }
	IObservable<object> KeyReleased { get; }
}