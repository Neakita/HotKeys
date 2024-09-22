namespace Katter.HotKeys.Behaviors;

public sealed class ActionDoubleTapBindingBehavior : DoubleTapBindingBehavior
{
	public ActionDoubleTapBindingBehavior(Action action)
	{
		_action = action;
	}

	protected override void OnDoubleTap()
	{
		_action();
	}

	private readonly Action _action;
}