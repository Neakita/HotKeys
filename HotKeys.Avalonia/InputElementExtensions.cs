using System.Reactive.Linq;
using Avalonia.Input;

namespace HotKeys.Avalonia;

public static class InputElementExtensions
{
	public static IObservable<InputState<object>> ObserveInputStates(this InputElement element)
	{
		return Observable.Merge(element.ObserveKeyStates().KeyAsObject(), element.ObserveMouseButtonStates().KeyAsObject());
	}
}