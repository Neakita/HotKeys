using CommunityToolkit.Diagnostics;

namespace HotKeys.ActionRunners;

internal sealed class CancellableActionRunner : ActionRunner
{
	public CancellableActionRunner(Action<CancellationToken> action)
	{
		_action = action;
	}

	public override void RunOnce()
	{
		_action.Invoke(new CancellationToken(true));
	}

	public override void BeginContinuousRun()
	{
		CancellationToken token;
		lock (_locker)
		{
			Guard.IsNull(_cancellationTokenSource);
			_cancellationTokenSource = new CancellationTokenSource();
			token = _cancellationTokenSource.Token;
		}
		_action(token);
		lock (_locker)
			_cancellationTokenSource = null;
	}

	public override void EndContinuousRun()
	{
		lock (_locker)
		{
			Guard.IsNotNull(_cancellationTokenSource);
			_cancellationTokenSource.Cancel();
		}
	}

	private readonly Action<CancellationToken> _action;
	private readonly object _locker = new();
	private CancellationTokenSource? _cancellationTokenSource;
}