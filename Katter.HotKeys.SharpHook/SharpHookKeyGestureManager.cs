using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SharpHook.Native;

namespace Katter.HotKeys.SharpHook;

public class SharpHookKeyGestureManager : GestureManager<KeyGesture>
{
	public IObservable<KeyGesture> GesturePressed => _gesturePressed.AsObservable();
	public IObservable<KeyGesture> GestureReleased => _gestureReleased.AsObservable();

	public SharpHookKeyGestureManager(KeyManager<KeyModifiers> modifiersManager, KeyManager<KeyCode> keyManager)
	{
		modifiersManager.KeyPressed
			.Subscribe(modifier => _pressedModifiers |= modifier)
			.DisposeWith(_constructorDisposable);
		modifiersManager.KeyReleased
			.Subscribe(modifier => _pressedModifiers &= ~modifier)
			.DisposeWith(_constructorDisposable);
		keyManager.KeyPressed.Where(IsNotModifier).Subscribe(OnKeyPressed).DisposeWith(_constructorDisposable);
		keyManager.KeyReleased.Where(IsNotModifier).Subscribe(OnKeyReleased).DisposeWith(_constructorDisposable);
	}

	private readonly CompositeDisposable _constructorDisposable = new();
	private readonly Subject<KeyGesture> _gesturePressed = new();
	private readonly Subject<KeyGesture> _gestureReleased = new();
	private readonly Dictionary<KeyCode, KeyModifiers> _pressedKeyModifiers = new();
	private KeyModifiers _pressedModifiers = KeyModifiers.None;

	private static bool IsNotModifier(KeyCode key)
	{
		return !SharpHookKeyModifiersManager.IsModifier(key);
	}

	private void OnKeyPressed(KeyCode key)
	{
		var modifiers = _pressedModifiers;
		if (!_pressedKeyModifiers.TryAdd(key, modifiers))
			return;
		_gesturePressed.OnNext(new KeyGesture(key, modifiers));
	}

	private void OnKeyReleased(KeyCode key)
	{
		var modifiers = _pressedKeyModifiers[key];
		_pressedKeyModifiers.Remove(key);
		_gestureReleased.OnNext(new KeyGesture(key, modifiers));
	}
}