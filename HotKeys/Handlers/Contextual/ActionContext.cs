namespace HotKeys.Handlers.Contextual;

public abstract class ActionContext
{
	public abstract bool IsAlive { get; }
	public abstract Task Elimination { get; }

	public abstract void WaitForElimination();
	public abstract bool IsEliminatedAfter(TimeSpan timeout);
	public abstract bool IsEliminatedAfterCompletion(Task task);
}