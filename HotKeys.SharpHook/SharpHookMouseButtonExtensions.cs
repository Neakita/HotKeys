using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public static class SharpHookMouseButtonExtensions
{
	public static IObservable<InputState<MouseButton>> ObserveMouseButtonStates(this IReactiveGlobalHook hook)
	{
		var keyPressed = hook.MousePressed.TransformArgs().ToPressedKeys();
		var keyReleased = hook.MouseReleased.TransformArgs().ToReleasedKeys();
		return keyPressed.Merge(keyReleased);
	}

	private static IObservable<MouseButton> TransformArgs(this IObservable<MouseHookEventArgs> source)
	{
		return source.Select(TransformArgs);
	}

	private static MouseButton TransformArgs(MouseHookEventArgs args)
	{
		return args.Data.Button;
	}
}