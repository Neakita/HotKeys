using System.Reactive.Linq;
using Avalonia.Input;

namespace HotKeys.Avalonia;

public sealed class InputElementKeyManager : KeyManager<Key>
{
	public IObservable<Key> KeyPressed =>
		Observable.FromEventPattern<KeyEventArgs>(
			handler => _element.KeyDown += handler,
			handler => _element.KeyDown -= handler)
		.Select(args => args.EventArgs.Key);

	public IObservable<Key> KeyReleased =>
		Observable.FromEventPattern<KeyEventArgs>(
			handler => _element.KeyUp += handler,
			handler => _element.KeyUp -= handler)
		.Select(args => args.EventArgs.Key);

	public InputElementKeyManager(InputElement element)
	{
		_element = element;
	}

	private readonly InputElement _element;
}