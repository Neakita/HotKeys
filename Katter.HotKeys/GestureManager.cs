namespace Katter.HotKeys;

public interface GestureManager<TGesture>
{
	public IObservable<TGesture> GesturePressed { get; }
	public IObservable<TGesture> GestureReleased { get; }
}