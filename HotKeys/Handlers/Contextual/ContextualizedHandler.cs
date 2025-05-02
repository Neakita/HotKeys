using CommunityToolkit.Diagnostics;

namespace HotKeys.Handlers.Contextual;

public sealed class ContextualizedHandler : ContinuousHandler
{
	public ContextualizedHandler(Action<ActionContext> action)
	{
		_action = action;
	}

	public void Begin()
	{
		if (!_previousRun.IsCompleted)
			return;
		Guard.IsNull(_currentContext);
		_currentContext = new ActionContext();
		_previousRun = Task.Run(() => _action(_currentContext));
	}

	public void End()
	{
		_currentContext?.Eliminate();
		_currentContext = null;
	}

	private readonly Action<ActionContext> _action;
	private Task _previousRun = Task.CompletedTask;
	private ActionContext? _currentContext;
}