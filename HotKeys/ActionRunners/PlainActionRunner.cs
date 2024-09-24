using CommunityToolkit.Diagnostics;

namespace HotKeys.ActionRunners;

internal sealed class PlainActionRunner : ActionRunner
{
	public PlainActionRunner(Action action)
	{
		_action = action;
	}

	public override void RunOnce()
	{
		_action();
	}

	public override void BeginContinuousRun()
	{
		lock (_locker)
		{
			Guard.IsNull(_shouldStop);
			_shouldStop = false;
		}
		while (_shouldStop == false)
			_action();
		lock (_locker)
		{
			Guard.IsNotNull(_shouldStop);
			Guard.IsTrue(_shouldStop.Value);
			_shouldStop = null;
		}
	}

	public override void EndContinuousRun()
	{
		lock (_locker)
		{
			Guard.IsNotNull(_shouldStop);
			_shouldStop = true;
		}
	}

	private readonly Action _action;
	private readonly object _locker = new();
	private bool? _shouldStop;
}