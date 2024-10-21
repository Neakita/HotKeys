using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public sealed class SharpHookMouseButtonsManager : KeyManager<FormattedButton>
{
	public IObservable<FormattedButton> KeyPressed { get; }
	public IObservable<FormattedButton> KeyReleased { get; }

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

	private static FormattedButton TransformToFormatted(MouseButton button)
	{
		return new FormattedButton(button);
	}
}