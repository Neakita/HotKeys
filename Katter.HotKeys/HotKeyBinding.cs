using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Katter.HotKeys.Behaviours;

namespace Katter.HotKeys;

public sealed class HotKeyBinding<TGesture> : IDisposable where TGesture : class
{
	public string Name { get; }

	public BindingBehaviour Behaviour
	{
		get => _behaviour;
		set
		{
			Guard.IsFalse(IsDisposed);
			var oldValue = _behaviour;
			_behaviour = value;
			BehaviorChangedEventArgs<TGesture> args = new(this, oldValue, value);
			_behaviourChanged.OnNext(args);
		}
	}

	public TGesture? Gesture
	{
		get => _gesture;
		set
		{
			Guard.IsFalse(IsDisposed);
			var oldValue = _gesture;
			_gesture = value;
			GestureChangedEventArgs<TGesture> args = new(this, oldValue, value);
			_gestureChanged.OnNext(args);
		}
	}

	public bool IsDisposed { get; private set; }

	public void Dispose()
	{
		if (IsDisposed)
			return;
		IsDisposed = true;
		_behaviourChanged.OnCompleted();
		_behaviourChanged.Dispose();
		_gestureChanged.OnCompleted();
		_gestureChanged.Dispose();
		_disposed.OnNext(this);
		_disposed.OnCompleted();
		_disposed.Dispose();
	}

	internal IObservable<BehaviorChangedEventArgs<TGesture>> BehaviourChanged => _behaviourChanged;
	internal IObservable<GestureChangedEventArgs<TGesture>> GestureChanged => _gestureChanged;
	internal IObservable<HotKeyBinding<TGesture>> Disposed => _disposed;

	internal HotKeyBinding(string name, BindingBehaviour behaviour, TGesture? gesture = null)
	{
		_behaviour = behaviour;
		Name = name;
		Gesture = gesture;
	}

	internal void OnPressed() => Behaviour.OnPressed();
	internal void OnReleased() => Behaviour.OnReleased();

	private readonly Subject<BehaviorChangedEventArgs<TGesture>> _behaviourChanged = new();
	private readonly Subject<GestureChangedEventArgs<TGesture>> _gestureChanged = new();
	private readonly Subject<HotKeyBinding<TGesture>> _disposed = new();
	private TGesture? _gesture;
	private BindingBehaviour _behaviour;
}