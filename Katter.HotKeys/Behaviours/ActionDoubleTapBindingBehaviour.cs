namespace Katter.HotKeys.Behaviours;

public sealed class ActionDoubleTapBindingBehaviour : DoubleTapBindingBehaviour
{
	public ActionDoubleTapBindingBehaviour(Action action)
	{
		_action = action;
	}

	protected override void OnDoubleTap()
	{
		_action();
	}

	private readonly Action _action;
}