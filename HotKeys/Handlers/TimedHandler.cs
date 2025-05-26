using CommunityToolkit.Diagnostics;

namespace HotKeys.Handlers;

public sealed class TimedHandler : ContinuousHandler
{
	public TimeSpan Period { get; set; }

	public TimedHandler(OnetimeHandler loopHandler, OnetimeHandler? loopEndHandler)
	{
		_loopHandler = loopHandler;
		_loopEndHandler = loopEndHandler;
	}

	public void Begin()
	{
		Guard.IsNull(_session);
		if (!_lookTask.IsCompleted)
			return;
		_session = new TimedHandlerSession();
		_lookTask = Task.Run(Loop);
	}

	public void End()
	{
		if (_session == null)
			return;
		_session.ShouldStop = true;
		_session = null;
	}

	private readonly OnetimeHandler _loopHandler;
	private readonly OnetimeHandler? _loopEndHandler;
	private Task _lookTask = Task.CompletedTask;
	private TimedHandlerSession? _session;

	private void Loop()
	{
		var session = _session;
		Guard.IsNotNull(session);
		while (!session.ShouldStop)
		{
			_loopHandler.Handle();
			session.AdvanceNextTimestamp(Period);
			session.SleepUntilNextTimestamp();
		}
		_loopEndHandler?.Handle();
	}
}