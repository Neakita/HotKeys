using Katter.HotKeys.Behaviours;

namespace Katter.HotKeys;

public sealed class BehaviorChangedEventArgs<TGesture> where TGesture : class
{
	public HotKeyBinding<TGesture> Sender { get; }
	public BindingBehaviour OldBehavior { get; }
	public BindingBehaviour NewBehavior { get; }

	public BehaviorChangedEventArgs(
		HotKeyBinding<TGesture> sender,
		BindingBehaviour oldBehavior,
		BindingBehaviour newBehavior)
	{
		Sender = sender;
		OldBehavior = oldBehavior;
		NewBehavior = newBehavior;
	}
}