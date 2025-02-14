using System.Reactive.Linq;
using Avalonia.Input;

namespace HotKeys.Avalonia;

public sealed class InputElementMouseButtonManager : KeyManager<MouseButton>
{
	public IObservable<MouseButton> KeyPressed =>
		Observable.FromEventPattern<PointerPressedEventArgs>(
				handler => _element.PointerPressed += handler,
				handler => _element.PointerPressed -= handler)
			.Select(args => ArgsToButton(args.EventArgs));

	public IObservable<MouseButton> KeyReleased =>
		Observable.FromEventPattern<PointerReleasedEventArgs>(
				handler => _element.PointerReleased += handler,
				handler => _element.PointerReleased -= handler)
			.Select(args => ArgsToButton(args.EventArgs));

	public InputElementMouseButtonManager(InputElement element)
	{
		_element = element;
	}

	private readonly InputElement _element;

	private static MouseButton ArgsToButton(PointerEventArgs args)
	{
		return args.GetCurrentPoint(null).Properties.PointerUpdateKind.GetMouseButton();
	}
}