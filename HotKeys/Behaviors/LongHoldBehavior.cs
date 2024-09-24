using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal sealed class LongHoldBehavior : Behavior
{
	private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(500);

	public LongHoldBehavior(ActionRunner actionRunner) : base(actionRunner)
	{
	}

	protected internal override void OnPressed()
	{
		Guard.IsNull(_cancellationTokenSource);
		Guard.IsNull(_invoked);
		_cancellationTokenSource = new CancellationTokenSource();
		_invoked = false;
		var token = _cancellationTokenSource.Token;
		Task.Run(() =>
		{
			Thread.Sleep(Interval);
			lock (_locker)
			{
				if (token.IsCancellationRequested)
					return;
				_invoked = true;
				Task.Run(ActionRunner.BeginContinuousRun);
			}
		}, token);
	}

	protected internal override void OnReleased()
	{
		lock (_locker)
		{
			Guard.IsNotNull(_cancellationTokenSource);
			Guard.IsNotNull(_invoked);
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource = null;
			if (_invoked.Value)
				ActionRunner.EndContinuousRun();
			_invoked = null;
		}
	}

	private readonly object _locker = new();
	private CancellationTokenSource? _cancellationTokenSource;
	private bool? _invoked;
}