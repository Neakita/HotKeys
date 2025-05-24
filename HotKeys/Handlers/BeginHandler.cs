namespace HotKeys.Handlers;

public sealed class BeginHandler : ContinuousHandler
{
	public BeginHandler(OnetimeHandler handler)
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