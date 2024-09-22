namespace Katter.HotKeys.Behaviors;

public sealed class ActionPressBindingBehavior : PressBindingBehavior
{
	public ActionPressBindingBehavior(Action action)
	{
		_action = action;
	}

	protected override void OnPress()
	{
		_action();
	}

	private readonly Action _action;
}