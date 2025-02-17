namespace HotKeys.Handlers.Behavioral;

internal sealed class ReleaseHandler : ContinuousHandler
{
	public ReleaseHandler(OnetimeHandler handler)
	{
		_handler = handler;
	}

	public void Begin()
	{
	}

	public void End()
	{
		_handler.Handle();
	}

	private readonly OnetimeHandler _handler;
}