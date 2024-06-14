using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace Katter.HotKeys.SharpHook;

public class SharpHookKeyboardKeyManager : KeyManager<KeyCode>
{
	public IObservable<KeyCode> KeyPressed => _hook.KeyPressed.Select(TransformArgs);
	public IObservable<KeyCode> KeyReleased => _hook.KeyReleased.Select(TransformArgs);

	public SharpHookKeyboardKeyManager(IReactiveGlobalHook hook)
	{
		_hook = hook;
	}

	private readonly IReactiveGlobalHook _hook;

	private static KeyCode TransformArgs(KeyboardHookEventArgs args)
	{
		return args.Data.KeyCode;
	}
}