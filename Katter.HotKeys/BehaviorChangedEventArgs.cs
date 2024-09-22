using Katter.HotKeys.Behaviors;

namespace Katter.HotKeys;

public sealed class BehaviorChangedEventArgs<TGesture> where TGesture : class
{
	public HotKeyBinding<TGesture> Sender { get; }
	public BindingBehavior OldBehavior { get; }
	public BindingBehavior NewBehavior { get; }

	public BehaviorChangedEventArgs(
		HotKeyBinding<TGesture> sender,
		BindingBehavior oldBehavior,
		BindingBehavior newBehavior)
	{
		Sender = sender;
		OldBehavior = oldBehavior;
		NewBehavior = newBehavior;
	}
}