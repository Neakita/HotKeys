using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal sealed class PressBehavior : Behavior
{
	public PressBehavior(ActionRunner actionRunner) : base(actionRunner)
	{
	}

	protected internal override void OnPressed()
	{
		ActionRunner.RunOnce();
	}
}