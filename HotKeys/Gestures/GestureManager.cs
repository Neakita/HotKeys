using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;

namespace HotKeys.Gestures;

public sealed class GestureManager : IDisposable
{
	public IObservable<Gesture> CurrentGestureChanged => _currentGestureChanged.AsObservable();

	public GestureManager(KeyManager keyManager)
	{
		keyManager.KeyPressed.Subscribe(AddToGesture);
		keyManager.KeyReleased.Subscribe(RemoveFromGesture);
	}

	public void Dispose()
	{
		_currentGestureChanged.Dispose();
	}

	private readonly Subject<Gesture> _currentGestureChanged = new();
	private Gesture _gesture = Gesture.Empty;

	private void AddToGesture(object key)
	{
		var builder = _gesture.Keys.ToBuilder();
		Guard.IsTrue(builder.Add(key));
		_gesture = new Gesture(builder.ToImmutable());
		_currentGestureChanged.OnNext(_gesture);
	}

	private void RemoveFromGesture(object key)
	{
		var builder = _gesture.Keys.ToBuilder();
		Guard.IsTrue(builder.Remove(key));
		_gesture = new Gesture(builder.ToImmutable());
		_currentGestureChanged.OnNext(_gesture);
	}
}