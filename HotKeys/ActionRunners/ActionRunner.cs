namespace HotKeys.ActionRunners;

internal abstract class ActionRunner
{
	public abstract void RunOnce();
	public abstract void BeginContinuousRun();
	public abstract void EndContinuousRun();
}