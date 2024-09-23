using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public sealed class SharpHookMouseButtonsManager : KeyManager<FormattedSharpHookMouseButton>
{
	public IObservable<FormattedSharpHookMouseButton> KeyPressed { get; }
	public IObservable<FormattedSharpHookMouseButton> KeyReleased { get; }

	public SharpHookMouseButtonsManager(IReactiveGlobalHook hook)
	{
		KeyPressed = hook.MousePressed
			.Select(TransformArgs)
			.Select(TransformToFormatted);
		KeyReleased = hook.MouseReleased
			.Select(TransformArgs)
			.Select(SwapButton2AndButton3) // https://github.com/TolikPylypchuk/SharpHook/issues/119
			.Select(TransformToFormatted);
	}

	private static MouseButton TransformArgs(MouseHookEventArgs args)
	{
		return args.Data.Button;
	}

	private static MouseButton SwapButton2AndButton3(MouseButton button)
	{
		if (button == MouseButton.Button2)
			return MouseButton.Button3;
		if (button == MouseButton.Button3)
			return MouseButton.Button2;
		return button;
	}

	private static FormattedSharpHookMouseButton TransformToFormatted(MouseButton button)
	{
		return (FormattedSharpHookMouseButton)button;
	}
}