using System.Reactive.Disposables;

namespace HotKeys;

internal static class Extensions
{
	public static void DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
	{
		compositeDisposable.Add(disposable);
	}

	public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		return source.Where(item => !predicate(item));
	}
}