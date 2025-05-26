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
		Task task = new(Loop);
		var session = new TimedHandlerSession
		{
			Task = task,
			NextTimestamp = DateTime.UtcNow
		};
		Guard.IsNull(_session);
		_session = session;
		task.Start();
	}

	public void End()
	{
		Guard.IsNotNull(_session);
		_session.ShouldStop = true;
		_session = null;
	}

	private readonly OnetimeHandler _loopHandler;
	private readonly OnetimeHandler? _loopEndHandler;
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