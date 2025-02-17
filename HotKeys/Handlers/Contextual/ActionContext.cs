namespace HotKeys.Handlers.Contextual;

public sealed class ActionContext
{
	public bool IsAlive => !IsEliminated;
	public bool IsEliminated { get; private set; }

	public Task Elimination => _taskCompletionSource.Task;

	public void WaitForElimination()
	{
		_taskCompletionSource.Task.Wait();
	}

	public bool IsEliminatedAfter(TimeSpan timeout)
	{
		if (timeout <= TimeSpan.Zero)
			return IsEliminated;
		return _taskCompletionSource.Task.Wait(timeout);
	}

	public bool IsAliveAfter(TimeSpan timeout)
	{
		return !IsEliminatedAfter(timeout);
	}

	public bool IsEliminatedAfterCompletion(Task task)
	{
		var completedTaskIndex = Task.WaitAny(_taskCompletionSource.Task, task);
		return completedTaskIndex == 0;
	}

	public bool IsAliveAfterCompletion(Task task)
	{
		return !IsEliminatedAfterCompletion(task);
	}

	internal void Eliminate()
	{
		IsEliminated = true;
		_taskCompletionSource.SetResult();
	}

	private readonly TaskCompletionSource _taskCompletionSource = new();
}