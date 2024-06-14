﻿using CommunityToolkit.Diagnostics;

namespace Katter.HotKeys.Behaviours;

public abstract class HoldBindingBehaviour : BindingBehaviour
{
	protected internal sealed override void OnPressed()
	{
		Guard.IsNull(_cancellationTokenSource);
		_cancellationTokenSource = new CancellationTokenSource();
		Task.Run(() => OnHold(_cancellationTokenSource.Token));
	}

	protected internal sealed override void OnReleased()
	{
		Guard.IsNotNull(_cancellationTokenSource);
		_cancellationTokenSource.Cancel();
		_cancellationTokenSource = null;
	}

	protected abstract void OnHold(CancellationToken cancellationToken);

	private CancellationTokenSource? _cancellationTokenSource;
}