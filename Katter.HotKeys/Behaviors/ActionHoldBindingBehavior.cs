namespace Katter.HotKeys.Behaviors;

public sealed class ActionHoldBindingBehavior : HoldBindingBehavior
{
	public ActionHoldBindingBehavior(Action<CancellationToken> action)
	{
		_action = action;
	}

	public ActionHoldBindingBehavior(Action action)
	{
		_action = cancellationToken =>
		{
			while (!cancellationToken.IsCancellationRequested)
				action();
		};
	}

	protected override void OnHold(CancellationToken cancellationToken)
	{
		_action(cancellationToken);
	}

	private readonly Action<CancellationToken> _action;
}