using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public sealed class SharpHookMouseButtonsManager : KeyManager<FormattedSharpButton>
{
	public IObservable<FormattedSharpButton> KeyPressed { get; }
	public IObservable<FormattedSharpButton> KeyReleased { get; }

	public SharpHookMouseButtonsManager(IReactiveGlobalHook hook)
	{
		KeyPressed = hook.MousePressed
			.Select(TransformArgs)
			.Select(TransformToFormatted);
		KeyReleased = hook.MouseReleased
			.Select(TransformArgs)
			.Select(TransformToFormatted);
	}

	private static MouseButton TransformArgs(MouseHookEventArgs args)
	{
		return args.Data.Button;
	}

	private static FormattedSharpButton TransformToFormatted(MouseButton button)
	{
		return new FormattedSharpButton(button);
	}
}