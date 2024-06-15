namespace Katter.HotKeys.Behaviours;

public sealed class ActionPressBindingBehaviour : PressBindingBehaviour
{
	public ActionPressBindingBehaviour(Action action)
	{
		_action = action;
	}

	protected override void OnPress()
	{
		_action();
	}

	private readonly Action _action;
}