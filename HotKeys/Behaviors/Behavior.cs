using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal abstract class Behavior
{
	public static Behavior Create(InputTypes inputType, ActionRunner actionRunner) => inputType switch
	{
		InputTypes.Tap => new TapBehavior(actionRunner),
		InputTypes.DoubleTap => new DoubleTapBehavior(actionRunner),
		InputTypes.Press => new PressBehavior(actionRunner),
		InputTypes.LongPress => new LongPressBehavior(actionRunner),
		InputTypes.Release => new ReleaseBehavior(actionRunner),
		InputTypes.Hold => new HoldBehavior(actionRunner),
		InputTypes.LongHold => new LongHoldBehavior(actionRunner),
		_ => throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null)
	};

	protected internal virtual void OnPressed()
	{
	}

	protected internal virtual void OnReleased()
	{
	}

	protected ActionRunner ActionRunner { get; }

	protected Behavior(ActionRunner actionRunner)
	{
		ActionRunner = actionRunner;
	}
}