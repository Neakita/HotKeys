using CommunityToolkit.Diagnostics;
using HotKeys.Bindings;

namespace HotKeys.ActionRunners;

internal sealed class ContextActionRunner : ActionRunner
{
	public ContextActionRunner(Action<ActionContext> action)
	{
		_action = action;
	}

	public void Initialize(Binding binding)
	{
		_binding = binding;
	}

	public override void RunOnce()
	{
		Guard.IsNotNull(_binding);
		_action(new UnbornActionContext(_binding));
	}

	public override void BeginContinuousRun()
	{
		Guard.IsNotNull(_binding);
		LivingActionContext context;
		lock (_locker)
		{
			Guard.IsNull(_currentContext);
			context = new LivingActionContext(_binding);
			_currentContext = context;
		}
		_action(context);
	}

	public override void EndContinuousRun()
	{
		lock (_locker)
		{
			Guard.IsNotNull(_currentContext);
			_currentContext.Eliminate();
			_currentContext = null;
		}
	}

	private readonly Action<ActionContext> _action;
	private readonly object _locker = new();
	private Binding? _binding;
	private LivingActionContext? _currentContext;
}