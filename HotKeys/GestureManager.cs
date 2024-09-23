using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;

namespace HotKeys;

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
	private readonly MutableGesture _gesture = new();

	private void AddToGesture(object key)
	{
		Guard.IsTrue(_gesture.Keys.Add(key));
		_currentGestureChanged.OnNext(_gesture);
	}

	private void RemoveFromGesture(object key)
	{
		Guard.IsTrue(_gesture.Keys.Remove(key));
		_currentGestureChanged.OnNext(_gesture);
	}
}