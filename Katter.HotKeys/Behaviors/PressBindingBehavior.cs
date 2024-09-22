namespace Katter.HotKeys.Behaviors;

public abstract class PressBindingBehavior : BindingBehavior
{
	protected internal sealed override void OnPressed()
	{
		Task.Run(OnPress);
	}

	protected internal sealed override void OnReleased()
	{
	}

	protected abstract void OnPress();
}