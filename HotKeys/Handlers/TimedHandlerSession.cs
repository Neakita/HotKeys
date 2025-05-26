namespace HotKeys.Handlers;

internal sealed class TimedHandlerSession
{
	public bool ShouldStop { get; set; }

	public void AdvanceNextTimestamp(TimeSpan value)
	{
		_nextTimestamp += value;
	}

	public void SleepUntilNextTimestamp()
	{
		var timeToSleep = _nextTimestamp - DateTime.UtcNow;
		if (timeToSleep > TimeSpan.Zero)
			Thread.Sleep(timeToSleep);
	}

	private DateTime _nextTimestamp = DateTime.UtcNow;
}