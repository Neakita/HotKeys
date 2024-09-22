namespace Katter.HotKeys.Behaviors;

public sealed class ActionLongHoldBindingBehavior : LongHoldBindingBehavior
{
	public ActionLongHoldBindingBehavior(Action<CancellationToken> action)
	{
		_action = action;
	}

	public ActionLongHoldBindingBehavior(Action action)
	{
		_action = cancellationToken =>
		{
			while (!cancellationToken.IsCancellationRequested)
				action();
		};
	}

	protected override void OnHold(CancellationToken cancellationToken) => _action(cancellationToken);

	private readonly Action<CancellationToken> _action;
}