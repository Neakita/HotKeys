namespace HotKeys.Handlers;

public sealed class BeginAsyncHandler : ContinuousHandler
{
	public BeginAsyncHandler(OnetimeHandler handler)
	{
		_handler = handler;
	}

	public void Begin()
	{
		if (_task.IsCompleted)
			_task = Task.Run(_handler.Handle);
	}

	public void End()
	{
	}

	private readonly OnetimeHandler _handler;
	private Task _task = Task.CompletedTask;
}