using CommunityToolkit.Diagnostics;

namespace HotKeys.Handlers.Behavioral;

internal sealed class LongHoldHandler : ContinuousHandler
{
	private static readonly TimeSpan Delay = TimeSpan.FromMilliseconds(500);

	public LongHoldHandler(ContinuousHandler handler)
	{
		_handler = handler;
	}

	public void Begin()
	{
		_cancellationTokenSource = new CancellationTokenSource();
		_ = BeginAsync(_cancellationTokenSource.Token);
	}

	private async Task BeginAsync(CancellationToken cancellationToken)
	{
		await Task.Delay(Delay, cancellationToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
		lock (_invokeLock)
		{
			if (cancellationToken.IsCancellationRequested)
				return;
			_handler.Begin();
			_isInvoked = true;	
		}
	}

	public void End()
	{
		Guard.IsNotNull(_cancellationTokenSource);
		lock (_invokeLock)
			_cancellationTokenSource.Cancel();
		if (_isInvoked)
			_handler.End();
		_isInvoked = false;
	}

	private readonly ContinuousHandler _handler;
	private readonly Lock _invokeLock = new();
	private CancellationTokenSource? _cancellationTokenSource;
	private bool _isInvoked;
}