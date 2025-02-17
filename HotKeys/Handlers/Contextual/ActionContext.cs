namespace HotKeys.Handlers.Contextual;

public sealed class ActionContext
{
	public bool IsAlive { get; private set; } = true;

	public Task Elimination => _taskCompletionSource.Task;

	public void WaitForElimination()
	{
		_taskCompletionSource.Task.Wait();
	}

	public bool IsEliminatedAfter(TimeSpan timeout)
	{
		if (timeout <= TimeSpan.Zero)
			return _taskCompletionSource.Task.IsCompleted;
		return _taskCompletionSource.Task.Wait(timeout);
	}

	public bool IsEliminatedAfterCompletion(Task task)
	{
		var completedTaskIndex = Task.WaitAny(_taskCompletionSource.Task, task);
		return completedTaskIndex == 0;
	}

	internal void Eliminate()
	{
		IsAlive = false;
		_taskCompletionSource.SetResult();
	}

	private readonly TaskCompletionSource _taskCompletionSource = new();
}