namespace Katter.HotKeys.Behaviours;

public sealed class ActionHoldBindingBehaviour : HoldBindingBehaviour
{
	public ActionHoldBindingBehaviour(Action<CancellationToken> action)
	{
		_action = action;
	}

	public ActionHoldBindingBehaviour(Action action)
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