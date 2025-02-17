namespace HotKeys.Handlers.Behavioral;

internal sealed class DoubleTapHandler : ContinuousHandler
{
	private static readonly TimeSpan MaximumPressDuration = TimeSpan.FromMilliseconds(125);
	private static readonly TimeSpan MaximumTapsInterval = TimeSpan.FromMilliseconds(500);

	public DoubleTapHandler(OnetimeHandler handler)
	{
		_handler = handler;
	}

	public void Begin()
	{
		_pressTime = DateTime.UtcNow;
	}

	public void End()
	{
		var interval = DateTime.UtcNow - _pressTime;
		if (interval <= MaximumPressDuration)
			OnTap();
	}

	private DateTime _pressTime;
	private DateTime _previousTapTime;
	private byte _tapsCount;
	private readonly OnetimeHandler _handler;

	private void OnTap()
	{
		var interval = DateTime.UtcNow - _previousTapTime;
		if (interval > MaximumTapsInterval)
			_tapsCount = 0;
		_tapsCount++;
		if (_tapsCount == 2)
		{
			_tapsCount = 0;
			_handler.Handle();
		}
		_previousTapTime = DateTime.UtcNow;
	}
}