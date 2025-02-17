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
		Guard.IsNull(_currentContext);
		_currentContext = new LivingActionContext();
		Task.Run(() => _action(_currentContext));
	}

	public void End()
	{
		Guard.IsNotNull(_currentContext);
		_currentContext.Eliminate();
		_currentContext = null;
	}

	private readonly Action<ActionContext> _action;
	private LivingActionContext? _currentContext;
}