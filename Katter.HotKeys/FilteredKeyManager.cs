using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Katter.HotKeys;

public abstract class FilteredKeyManager<TKey> : KeyManager<TKey>, IDisposable
{
	public IObservable<TKey> KeyPressed => _keyPressed.AsObservable();
	public IObservable<TKey> KeyReleased => _keyReleased.AsObservable();

	public void Dispose()
	{
		_disposable.Dispose();
		_keyPressed.Dispose();
		_keyReleased.Dispose();
	}

	protected FilteredKeyManager(IObservable<TKey> keyPressed, IObservable<TKey> keyReleased)
	{
		keyPressed
			.Where(_pressedKeys.Add)
			.Subscribe(_keyPressed)
			.DisposeWith(_disposable);
		keyReleased
			.Where(_pressedKeys.Remove)
			.Subscribe(_keyReleased)
			.DisposeWith(_disposable);
	}

	private readonly CompositeDisposable _disposable = new();
	private readonly HashSet<TKey> _pressedKeys = new();
	private readonly Subject<TKey> _keyPressed = new();
	private readonly Subject<TKey> _keyReleased = new();
}