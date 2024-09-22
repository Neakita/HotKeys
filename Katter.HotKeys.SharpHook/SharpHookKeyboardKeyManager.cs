using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace Katter.HotKeys.SharpHook;

public class SharpHookKeyboardKeyManager : KeyManager<KeyCode>, IDisposable
{
	public IObservable<KeyCode> KeyPressed => _keyPressed.AsObservable();
	public IObservable<KeyCode> KeyReleased => _keyReleased.AsObservable();

	public SharpHookKeyboardKeyManager(IReactiveGlobalHook hook)
	{
		hook.KeyPressed
			.Select(TransformArgs)
			.Where(_pressedKeys.Add)
			.Subscribe(_keyPressed)
			.DisposeWith(_disposable);
		hook.KeyReleased
			.Select(TransformArgs)
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
	private readonly HashSet<KeyCode> _pressedKeys = new();
	private readonly Subject<KeyCode> _keyPressed = new();
	private readonly Subject<KeyCode> _keyReleased = new();

	private static KeyCode TransformArgs(KeyboardHookEventArgs args)
	{
		return args.Data.KeyCode;
	}
}