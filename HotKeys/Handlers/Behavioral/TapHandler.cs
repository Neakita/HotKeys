namespace HotKeys.Handlers.Behavioral;

internal sealed class TapHandler : ContinuousHandler
{
	private static readonly TimeSpan MaximumInterval = TimeSpan.FromMilliseconds(125);

	public TapHandler(OnetimeHandler handler)
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
		if (interval <= MaximumInterval)
			_handler.Handle();
	}

	private readonly OnetimeHandler _handler;
	private DateTime _pressTime;
}