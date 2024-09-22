namespace Katter.HotKeys.Behaviors;

public sealed class ActionReleaseBindingBehavior : ReleaseBindingBehavior
{
	public ActionReleaseBindingBehavior(Action action)
	{
		_action = action;
	}

	protected override void OnRelease()
	{
		_action();
	}

	private readonly Action _action;
}