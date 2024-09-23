using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HotKeys;

public sealed class KeyManagerFilter<TKey> : KeyManager<TKey>, IDisposable where TKey : notnull
{
	public IObservable<TKey> KeyPressed => _keyPressed.AsObservable();
	public IObservable<TKey> KeyReleased => _keyReleased.AsObservable();

	public KeyManagerFilter(KeyManager<TKey> keyManager)
	{
		keyManager.KeyPressed
			.Where(_pressedKeys.Add)
			.Subscribe(_keyPressed)
			.DisposeWith(_disposable);
		keyManager.KeyReleased
			.Where(_pressedKeys.Remove)
			.Subscribe(_keyReleased)
			.DisposeWith(_disposable);
	}

	public void Dispose()
	{
		_disposable.Dispose();
		_keyPressed.Dispose();
		_keyReleased.Dispose();
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly HashSet<TKey> _pressedKeys = new();
	private readonly Subject<TKey> _keyPressed = new();
	private readonly Subject<TKey> _keyReleased = new();
}