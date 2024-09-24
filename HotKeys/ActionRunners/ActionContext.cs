using HotKeys.Bindings;

namespace HotKeys.ActionRunners;

public abstract class ActionContext
{
	public Binding Binding { get; }
	public abstract bool Alive { get; }

	public abstract void WaitForElimination();

	protected internal ActionContext(Binding binding)
	{
		Binding = binding;
	}
}