using System.Reactive.Linq;
using Avalonia.Input;

namespace HotKeys.Avalonia;

public static class InputElementKeyExtensions
{
	public static IObservable<InputState<Key>> ObserveKeyStates(this InputElement element)
	{
		var keyPressed = element
			.KeyPressed()
			.ToPressedKeys();
		var keyReleased = element
			.KeyReleased()
			.ToReleasedKeys();
		return keyPressed.Merge(keyReleased);
	}

	private static IObservable<Key> KeyPressed(this InputElement element) =>
		Observable.FromEventPattern<KeyEventArgs>(
				handler => element.KeyDown += handler,
				handler => element.KeyDown -= handler)
			.Select(args => args.EventArgs.Key);

	private static IObservable<Key> KeyReleased(this InputElement element) =>
		Observable.FromEventPattern<KeyEventArgs>(
				handler => element.KeyUp += handler,
				handler => element.KeyUp -= handler)
			.Select(args => args.EventArgs.Key);
}