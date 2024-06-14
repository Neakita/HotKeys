namespace Katter.HotKeys.Behaviours;

public class ActionTapBindingBehaviour : TapBindingBehaviour
{
	public ActionTapBindingBehaviour(Action action)
	{
		_action = action;
	}

	protected override void OnTap()
	{
		_action();
	}

	private readonly Action _action;
}