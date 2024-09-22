namespace Katter.HotKeys.Behaviors;

public class ActionTapBindingBehavior : TapBindingBehavior
{
	public ActionTapBindingBehavior(Action action)
	{
		_action = action;
	}

	protected override void OnTap()
	{
		_action();
	}

	private readonly Action _action;
}