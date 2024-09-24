using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal sealed class ReleaseBehavior : Behavior
{
	public ReleaseBehavior(ActionRunner actionRunner) : base(actionRunner)
	{
	}

	protected internal override void OnReleased()
	{
		ActionRunner.RunOnce();
	}
}