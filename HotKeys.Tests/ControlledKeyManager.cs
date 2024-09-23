using System.Reactive.Subjects;

namespace HotKeys.Tests;

internal sealed class ControlledKeyManager : KeyManager, IDisposable
{
	public IObservable<object> KeyPressed => _keyPressed;
	public IObservable<object> KeyReleased => _keyReleased;

	public void NotifyPressed(object key)
	{
		_keyPressed.OnNext(key);
	}

	public void NotifyReleased(object key)
	{
		_keyReleased.OnNext(key);
	}

	public void Dispose()
	{
		_keyPressed.Dispose();
		_keyReleased.Dispose();
	}

	private readonly Subject<object> _keyPressed = new();
	private readonly Subject<object> _keyReleased = new();
}