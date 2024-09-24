using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal sealed class TapBehavior : Behavior
{
	public TapBehavior(ActionRunner actionRunner) : base(actionRunner)
	{
	}

	protected internal override void OnPressed()
	{
		Guard.IsNull(_pressTime);
		_pressTime = DateTime.UtcNow;
	}

	protected internal override void OnReleased()
	{
		Guard.IsNotNull(_pressTime);
		var interval = DateTime.UtcNow - _pressTime.Value;
		if (interval <= MaximumInterval)
			ActionRunner.RunOnce();
		_pressTime = null;
	}

	private static readonly TimeSpan MaximumInterval = TimeSpan.FromMilliseconds(125);
	private DateTime? _pressTime;
}