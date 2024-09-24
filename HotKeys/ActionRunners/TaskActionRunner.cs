using CommunityToolkit.Diagnostics;

namespace HotKeys.ActionRunners;

internal sealed class TaskActionRunner : ActionRunner
{
	public TaskActionRunner(Action<Task> action)
	{
		_action = action;
	}

	public override void RunOnce()
	{
		_action(Task.CompletedTask);
	}

	public override void BeginContinuousRun()
	{
		Guard.IsNull(_taskCompletionSource);
		_taskCompletionSource = new TaskCompletionSource();
		_action(_taskCompletionSource.Task);
	}

	public override void EndContinuousRun()
	{
		Guard.IsNotNull(_taskCompletionSource);
		_taskCompletionSource.SetResult();
		_taskCompletionSource = null;
	}

	private readonly Action<Task> _action;
	private TaskCompletionSource? _taskCompletionSource;
}