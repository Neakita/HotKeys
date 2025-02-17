using System.Reactive.Linq;
using HotKeys.Gestures;

namespace HotKeys;

public static class ObservableInputStateExtensions
{
	public static IObservable<InputState<TKey>> Filter<TKey>(this IObservable<InputState<TKey>> source)
	{
		HashSet<TKey> pressedKeys = new();
		return source
			.Where(state => state.IsPressed ? pressedKeys.Add(state.Key) : pressedKeys.Remove(state.Key))
			.OnUnsubscribe(() => pressedKeys.Clear())
			.Publish()
			.RefCount();
	}

	public static IObservable<InputState<object>> KeyAsObject<TKey>(this IObservable<InputState<TKey>> source)
		where TKey : notnull
	{
		return source.Select(state => new ImmutableInputState<object>(state.Key, state.IsPressed));
	}

	public static IObservable<Gesture> ToGesture(this IObservable<InputState<object>> source)
	{
		var gesture = Gesture.Empty;
		return source
			.Select(state =>
			{
				gesture = state.IsPressed ? gesture.With(state.Key) : gesture.Without(state.Key);
				return gesture;
			})
			.Publish()
			.RefCount();
	}
}