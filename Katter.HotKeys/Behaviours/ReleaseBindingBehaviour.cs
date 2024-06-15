namespace Katter.HotKeys.Behaviours;

public abstract class ReleaseBindingBehaviour : BindingBehaviour
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