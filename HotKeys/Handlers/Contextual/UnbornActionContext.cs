namespace HotKeys.Handlers.Contextual;

internal sealed class UnbornActionContext : ActionContext
{
	public override bool IsAlive => false;
	public override Task Elimination => Task.CompletedTask;

	public override void WaitForElimination()
	{
	}

	public override bool IsEliminatedAfter(TimeSpan timeout)
	{
		return true;
	}
}