namespace Katter.HotKeys;

public interface KeyManager<T>
{
	public IObservable<T> KeyPressed { get; }
	public IObservable<T> KeyReleased { get; }
}