using HotKeys.Bindings;

namespace HotKeys.ActionRunners;

public abstract class ActionContext
{
	public Binding Binding { get; }
	public abstract bool Alive { get; }
	public abstract Task Elimination { get; }

	public abstract void WaitForElimination();
	public abstract bool WaitForElimination(TimeSpan timeout);

	protected internal ActionContext(Binding binding)
	{
		Binding = binding;
	}
}