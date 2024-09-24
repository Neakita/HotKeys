using HotKeys.Bindings;

namespace HotKeys.ActionRunners;

internal sealed class UnbornActionContext : ActionContext
{
	public override bool Alive => false;

	public override void WaitForElimination()
	{
	}

	public override bool WaitForElimination(TimeSpan timeout)
	{
		return true;
	}

	internal UnbornActionContext(Binding binding) : base(binding)
	{
	}
}