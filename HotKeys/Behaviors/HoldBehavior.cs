using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal sealed class HoldBehavior : Behavior
{
	public HoldBehavior(ActionRunner actionRunner) : base(actionRunner)
	{
	}

	protected internal override void OnPressed()
	{
		Task.Run(ActionRunner.BeginContinuousRun);
	}

	protected internal override void OnReleased()
	{
		Task.Run(ActionRunner.EndContinuousRun);
	}
}