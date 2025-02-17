namespace HotKeys.Handlers.Contextual;

internal sealed class LivingActionContext : ActionContext
{
	public override bool IsAlive => _isAlive;
	public override Task Elimination => _taskCompletionSource.Task;

	public override void WaitForElimination()
	{
		_taskCompletionSource.Task.Wait();
	}

	public override bool IsEliminatedAfter(TimeSpan timeout)
	{
		if (timeout <= TimeSpan.Zero)
			return _taskCompletionSource.Task.IsCompleted;
		return _taskCompletionSource.Task.Wait(timeout);
	}

	public override bool IsEliminatedAfterCompletion(Task task)
	{
		var completedTaskIndex = Task.WaitAny(_taskCompletionSource.Task, task);
		return completedTaskIndex == 0;
	}

	internal void Eliminate()
	{
		_isAlive = false;
		_taskCompletionSource.SetResult();
	}

	private readonly TaskCompletionSource _taskCompletionSource = new();
	private bool _isAlive = true;
}