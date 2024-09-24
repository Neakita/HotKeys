using HotKeys.Bindings;

namespace HotKeys.ActionRunners;

internal sealed class UnbornActionContext : ActionContext
{
	public override bool Alive => false;

	public override void WaitForElimination()
	{
	}

	internal UnbornActionContext(Binding binding) : base(binding)
	{
	}
}