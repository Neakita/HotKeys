namespace Katter.HotKeys.Behaviours;

public abstract class PressBindingBehaviour : BindingBehaviour
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