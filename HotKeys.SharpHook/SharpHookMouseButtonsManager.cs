using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public sealed class SharpHookMouseButtonsManager : KeyManager<MouseButton>
{
	public IObservable<MouseButton> KeyPressed { get; }
	public IObservable<MouseButton> KeyReleased { get; }

	public SharpHookMouseButtonsManager(IReactiveGlobalHook hook)
	{
		KeyPressed = hook.MousePressed.Select(TransformArgs);
		KeyReleased = hook.MouseReleased.Select(TransformArgs)
			// https://github.com/TolikPylypchuk/SharpHook/issues/119
			.Select(button =>
			{
				if (button == MouseButton.Button2)
					return MouseButton.Button3;
				if (button == MouseButton.Button3)
					return MouseButton.Button2;
				return button;
			});
	}

	private static MouseButton TransformArgs(MouseHookEventArgs args)
	{
		return args.Data.Button;
	}
}