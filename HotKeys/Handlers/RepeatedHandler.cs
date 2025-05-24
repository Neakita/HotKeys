namespace HotKeys.Handlers;

public sealed class RepeatedHandler : ContinuousHandler
{
	public RepeatedHandler(OnetimeHandler loopHandler, OnetimeHandler? loopEndHandler = null)
	{
		_loopHandler = loopHandler;
		_loopEndHandler = loopEndHandler;
	}

	public void Begin()
	{
		if (!_task.IsCompleted)
			return;
		_shouldStop = false;
		_task = Task.Run(Loop);
	}

	public void End()
	{
		_shouldStop = true;
	}

	private readonly OnetimeHandler _loopHandler;
	private readonly OnetimeHandler? _loopEndHandler;
	private Task _task = Task.CompletedTask;
	private bool _shouldStop;

	private void Loop()
	{
		while (!_shouldStop)
			_loopHandler.Handle();
		_loopEndHandler?.Handle();
	}
}