namespace HotKeys.Handlers;

internal sealed class TimedHandlerSession
{
	public DateTime NextTimestamp { get; set; }
	public required Task Task { get; init; }
	public bool ShouldStop { get; set; }

	public void SleepUntilNextTimestamp()
	{
		var timeToSleep = NextTimestamp - DateTime.UtcNow;
		if (timeToSleep > TimeSpan.Zero)
			Thread.Sleep(timeToSleep);
	}

	public void AdvanceNextTimestamp(TimeSpan value)
	{
		NextTimestamp += value;
	}
}