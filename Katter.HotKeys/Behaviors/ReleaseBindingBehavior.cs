namespace Katter.HotKeys.Behaviors;

public abstract class ReleaseBindingBehavior : BindingBehavior
{
	protected internal sealed override void OnPressed()
	{
	}

	protected internal sealed override void OnReleased()
	{
		Task.Run(OnReleased);
	}

	protected abstract void OnRelease();
}