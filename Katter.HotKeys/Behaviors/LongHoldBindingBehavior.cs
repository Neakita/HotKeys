using CommunityToolkit.Diagnostics;

namespace Katter.HotKeys.Behaviors;

public abstract class LongHoldBindingBehavior : BindingBehavior
{
	public TimeSpan Delay { get; set; } = TimeSpan.FromMilliseconds(500);

	protected internal sealed override void OnPressed()
	{
		Guard.IsNull(_cancellationTokenSource);
		CancellationTokenSource cancellationTokenSource = new();
		_cancellationTokenSource = cancellationTokenSource;
		Task.Run(() =>
		{
			Thread.Sleep(Delay);
			if (!cancellationTokenSource.IsCancellationRequested)
				OnHold(cancellationTokenSource.Token);
		});
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