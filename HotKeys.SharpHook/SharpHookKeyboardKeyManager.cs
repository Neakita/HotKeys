using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public class SharpHookKeyboardKeyManager : KeyManager<FormattedSharpHookKeyCode>
{
	public IObservable<FormattedSharpHookKeyCode> KeyPressed =>
		_hook.KeyPressed
			.Select(TransformArgs)
			.Select(TransformToFormatted);

	public IObservable<FormattedSharpHookKeyCode> KeyReleased =>
		_hook.KeyReleased
			.Select(TransformArgs)
			.Select(TransformToFormatted);

	public SharpHookKeyboardKeyManager(IReactiveGlobalHook hook)
	{
		_hook = hook;
	}

	private readonly IReactiveGlobalHook _hook;

	private static KeyCode TransformArgs(KeyboardHookEventArgs args)
	{
		return args.Data.KeyCode;
	}

	private static FormattedSharpHookKeyCode TransformToFormatted(KeyCode keyCode)
	{
		return new FormattedSharpHookKeyCode(keyCode);
	}
}