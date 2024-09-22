namespace Katter.HotKeys.Behaviors;

public abstract class BindingBehavior
{
	protected internal abstract void OnPressed();
	protected internal abstract void OnReleased();
}