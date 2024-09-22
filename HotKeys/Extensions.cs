﻿using System.Reactive.Disposables;

namespace HotKeys;

internal static class Extensions
{
	public static void DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
	{
		compositeDisposable.Add(disposable);
	}
}