using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace HotKeys;

public static class ObservableExtensions
{
	public static IObservable<T> OnUnsubscribe<T>(this IObservable<T> source, Action callback)
	{
		return Observable.Create<T>(observer =>
		{
			var subscription = source.Subscribe(observer);
			var callbackDisposable = Disposable.Create(callback);
			return new CompositeDisposable(subscription, callbackDisposable);
		});
	}

	public static IObservable<InputState<TKey>> ToPressedKeys<TKey>(this IObservable<TKey> source) where TKey : notnull
	{
		return source.Select<TKey, InputState<TKey>>(static key => new ImmutableInputState<TKey>(key, true));
	}

	public static IObservable<InputState<TKey>> ToReleasedKeys<TKey>(this IObservable<TKey> source) where TKey : notnull
	{
		return source.Select<TKey, InputState<TKey>>(static key => new ImmutableInputState<TKey>(key, false));
	}
}