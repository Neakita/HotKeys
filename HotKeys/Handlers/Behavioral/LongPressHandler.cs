using CommunityToolkit.Diagnostics;

namespace HotKeys.Handlers.Behavioral;

internal sealed class LongPressHandler : ContinuousHandler
{
	private static readonly TimeSpan HoldDuration = TimeSpan.FromMilliseconds(500);

	public LongPressHandler(OnetimeHandler handler)
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
		await Task.Delay(HoldDuration, cancellationToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
		if (cancellationToken.IsCancellationRequested)
			return;
		_handler.Handle();
	}

	public void End()
	{
		Guard.IsNotNull(_cancellationTokenSource);
		_cancellationTokenSource.Cancel();
	}

	private readonly OnetimeHandler _handler;
	private CancellationTokenSource? _cancellationTokenSource;
}