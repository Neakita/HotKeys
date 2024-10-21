using HotKeys.Bindings;

namespace HotKeys.ActionRunners;

internal sealed class LivingActionContext : ActionContext
{
	public override bool Alive => _alive;
	public override Task Elimination => _taskCompletionSource.Task;

	public override void WaitForElimination()
	{
		_taskCompletionSource.Task.Wait();
	}

	public override bool WaitForElimination(TimeSpan timeout)
	{
		if (timeout <= TimeSpan.Zero)
			return _taskCompletionSource.Task.IsCompleted;
		return _taskCompletionSource.Task.Wait(timeout);
	}

	internal LivingActionContext(Binding binding) : base(binding)
	{
	}

	internal void Eliminate()
	{
		_alive = false;
		_taskCompletionSource.SetResult();
	}

	private readonly TaskCompletionSource _taskCompletionSource = new();
	private bool _alive = true;
}