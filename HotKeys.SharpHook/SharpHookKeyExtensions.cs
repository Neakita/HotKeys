using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public static class SharpHookKeyExtensions
{
	public static IObservable<InputState<KeyCode>> ObserveKeyStates(this IReactiveGlobalHook hook)
	{
		var keyPressed = hook.KeyPressed.TransformArgs().ToPressedKeys();
		var keyReleased = hook.KeyReleased.TransformArgs().ToReleasedKeys();
		return keyPressed.Merge(keyReleased);
	}

	private static IObservable<KeyCode> TransformArgs(this IObservable<KeyboardHookEventArgs> source)
	{
		return source.Select(TransformArgs);
	}

	private static KeyCode TransformArgs(KeyboardHookEventArgs args)
	{
		return args.Data.KeyCode;
	}
}