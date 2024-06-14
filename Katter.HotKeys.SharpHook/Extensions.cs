using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Katter.HotKeys.SharpHook;

internal static class Extensions
{
	public static void DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
	{
		compositeDisposable.Add(disposable);
	}

	public static IObservable<T> NotNull<T>(this IObservable<T?> observable)
	{
		return observable.Where(item => item != null).Select(item => item!);
	}
}