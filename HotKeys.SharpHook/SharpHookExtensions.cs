using System.Reactive.Linq;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public static class SharpHookExtensions
{
	public static IObservable<InputState<object>> ObserveInputStates(this IReactiveGlobalHook hook)
	{
		return Observable.Merge(hook.ObserveKeyStates().KeyAsObject(), hook.ObserveMouseButtonStates().KeyAsObject());
	}
}