namespace Katter.HotKeys.Behaviours;

public sealed class ActionReleaseBindingBehaviour : ReleaseBindingBehaviour
{
	public ActionReleaseBindingBehaviour(Action action)
	{
		_action = action;
	}

	protected override void OnRelease()
	{
		_action();
	}

	private readonly Action _action;
}