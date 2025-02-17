using System.Reactive.Linq;
using Avalonia.Input;

namespace HotKeys.Avalonia;

public static class InputElementMouseButtonExtensions
{
	public static IObservable<InputState<MouseButton>> ObserveMouseButtonStates(this InputElement element)
	{
		var keyPressed = element
			.MouseButtonPressed()
			.Select<MouseButton, InputState<MouseButton>>(static button => new ImmutableInputState<MouseButton>(button, true));
		var keyReleased = element
			.MouseButtonReleased()
			.Select<MouseButton, InputState<MouseButton>>(static key => new ImmutableInputState<MouseButton>(key, false));
		return keyPressed.Merge(keyReleased);
	}

	private static IObservable<MouseButton> MouseButtonPressed(this InputElement element) =>
		Observable.FromEventPattern<PointerPressedEventArgs>(
				handler => element.PointerPressed += handler,
				handler => element.PointerPressed -= handler)
			.Select(args => ArgsToButton(args.EventArgs))
			.Where(button => button != MouseButton.None);

	private static IObservable<MouseButton> MouseButtonReleased(this InputElement element) =>
		Observable.FromEventPattern<PointerReleasedEventArgs>(
				handler => element.PointerReleased += handler,
				handler => element.PointerReleased -= handler)
			.Select(args => ArgsToButton(args.EventArgs))
			.Where(button => button != MouseButton.None);

	private static MouseButton ArgsToButton(PointerEventArgs args)
	{
		return args.GetCurrentPoint(null).Properties.PointerUpdateKind.GetMouseButton();
	}
}