using CommunityToolkit.Diagnostics;

namespace Katter.HotKeys.Behaviors;

public abstract class DoubleTapBindingBehavior : BindingBehavior
{
	public TimeSpan MaximumPressAndReleaseInterval { get; set; } = TimeSpan.FromMilliseconds(125);

	public TimeSpan MaximumTapsInterval { get; set; } = TimeSpan.FromMilliseconds(500);

	protected internal sealed override void OnPressed()
	{
		Guard.IsNull(_pressTime);
		_pressTime = DateTime.UtcNow;
	}

	protected internal sealed override void OnReleased()
	{
		Guard.IsNotNull(_pressTime);
		var interval = DateTime.UtcNow - _pressTime.Value;
		if (interval <= MaximumPressAndReleaseInterval)
			OnTap();
		_pressTime = null;
	}

	protected abstract void OnDoubleTap();

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
			Task.Run(OnDoubleTap);
		}
		_previousTapTime = DateTime.UtcNow;
	}
}