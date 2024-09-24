using System.Reactive.Disposables;

namespace HotKeys;

internal static class Extensions
{
	public static void DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
	{
		compositeDisposable.Add(disposable);
	}

	public static IEnumerable<KeyValuePair<TKey, TValue>> WhereKeyNot<TKey, TValue>(
		this IEnumerable<KeyValuePair<TKey, TValue>> source,
		Func<TKey, bool> predicate)
	{
		return source.Where(pair => !predicate(pair.Key));
	}
}