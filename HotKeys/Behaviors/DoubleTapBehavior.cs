using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;

namespace HotKeys.Behaviors;

internal sealed class DoubleTapBehavior : Behavior
{
	public DoubleTapBehavior(ActionRunner actionRunner) : base(actionRunner)
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
		if (interval <= MaximumPressAndReleaseInterval)
			OnTap();
		_pressTime = null;
	}

	private static readonly TimeSpan MaximumPressAndReleaseInterval = TimeSpan.FromMilliseconds(125);
	private static readonly TimeSpan MaximumTapsInterval = TimeSpan.FromMilliseconds(500);
	private DateTime? _pressTime;
	private DateTime _previousTapTime;
	private byte _tapsCount;

	private void OnTap()
	{
		var interval = DateTime.UtcNow - _previousTapTime;
		if (interval > MaximumTapsInterval)
			_tapsCount = 0;
		_tapsCount++;
		if (_tapsCount == 2)
		{
			_tapsCount = 0;
			Task.Run(ActionRunner.RunOnce);
		}
		_previousTapTime = DateTime.UtcNow;
	}
}