namespace HotKeys;

public interface KeyManager<out T>
{
	public IObservable<T> KeyPressed { get; }
	public IObservable<T> KeyReleased { get; }
}