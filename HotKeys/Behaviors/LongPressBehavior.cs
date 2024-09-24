using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal sealed class LongPressBehavior : Behavior
{
	private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(500);

	public LongPressBehavior(ActionRunner actionRunner) : base(actionRunner)
	{
	}

	protected internal override void OnPressed()
	{
		Guard.IsNull(_cancellationTokenSource);
		_cancellationTokenSource = new CancellationTokenSource();
		var token = _cancellationTokenSource.Token;
		Task.Run(() =>
		{
			Thread.Sleep(Interval);
			if (!token.IsCancellationRequested)
				ActionRunner.RunOnce();
		}, token);
	}

	protected internal override void OnReleased()
	{
		Guard.IsNotNull(_cancellationTokenSource);
		_cancellationTokenSource.Cancel();
		_cancellationTokenSource = null;
	}

	private CancellationTokenSource? _cancellationTokenSource;
}