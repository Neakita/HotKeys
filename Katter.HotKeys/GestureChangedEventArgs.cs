namespace Katter.HotKeys;

public sealed class GestureChangedEventArgs<TGesture> where TGesture : class
{
	public HotKeyBinding<TGesture> Sender { get; }
	public TGesture? OldGesture { get; }
	public TGesture? NewGesture { get; }

	public GestureChangedEventArgs(HotKeyBinding<TGesture> sender, TGesture? oldGesture, TGesture? newGesture)
	{
		Sender = sender;
		OldGesture = oldGesture;
		NewGesture = newGesture;
	}
}