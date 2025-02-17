namespace HotKeys.Handlers.Behavioral;

internal sealed class PressHandler : ContinuousHandler
{
	public PressHandler(OnetimeHandler handler)
	{
		_handler = handler;
	}

	public void Begin()
	{
		_handler.Handle();
	}

	public void End()
	{
	}

	private readonly OnetimeHandler _handler;
}