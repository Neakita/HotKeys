using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace Katter.HotKeys.SharpHook;

public sealed class SharpHookMouseButtonsManager : KeyManager<MouseButton>
{
	public IObservable<MouseButton> KeyPressed { get; }
	public IObservable<MouseButton> KeyReleased { get; }

	public SharpHookMouseButtonsManager(IReactiveGlobalHook hook)
	{
		KeyPressed = hook.MousePressed.Select(TransformArgs);
		KeyReleased = hook.MouseReleased.Select(TransformArgs);
	}

	private static MouseButton TransformArgs(MouseHookEventArgs args)
	{
		return args.Data.Button;
	}
}