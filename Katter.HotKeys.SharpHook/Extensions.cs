using System.Reactive.Disposables;

namespace Katter.HotKeys.SharpHook;

internal static class Extensions
{
	public static void DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
	{
		compositeDisposable.Add(disposable);
	}
}