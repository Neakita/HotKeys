using System.Reactive.Subjects;

namespace HotKeys.Tests;

internal sealed class ControlledKeyManager<T> : KeyManager<T>, IDisposable where T : notnull
{
	public IObservable<T> KeyPressed => _keyPressed;
	public IObservable<T> KeyReleased => _keyReleased;

	public void NotifyPressed(T key)
	{
		_keyPressed.OnNext(key);
	}

	public void NotifyReleased(T key)
	{
		_keyReleased.OnNext(key);
	}

	public void Dispose()
	{
		_keyPressed.Dispose();
		_keyReleased.Dispose();
	}

	private readonly Subject<T> _keyPressed = new();
	private readonly Subject<T> _keyReleased = new();
}