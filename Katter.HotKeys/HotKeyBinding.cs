using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using Katter.HotKeys.Behaviors;

namespace Katter.HotKeys;

public sealed class HotKeyBinding<TGesture> : IDisposable where TGesture : class
{
	public BindingBehavior Behavior
	{
		get => _behavior;
		set
		{
			Guard.IsFalse(IsDisposed);
			BehaviorChangedEventArgs<TGesture> args = new(this, _behavior, value);
			_behavior = value;
			_behaviorChanged.OnNext(args);
		}
	}

	public TGesture? Gesture
	{
		get => _gesture;
		set
		{
			Guard.IsFalse(IsDisposed);
			GestureChangedEventArgs<TGesture> args = new(this, _gesture, value);
			_gesture = value;
			_gestureChanged.OnNext(args);
		}
	}

	public bool IsDisposed { get; private set; }

	public void Dispose()
	{
		if (IsDisposed)
			return;
		IsDisposed = true;
		_behaviorChanged.OnCompleted();
		_behaviorChanged.Dispose();
		_gestureChanged.OnCompleted();
		_gestureChanged.Dispose();
		_disposed.OnNext(this);
		_disposed.OnCompleted();
		_disposed.Dispose();
	}

	internal IObservable<BehaviorChangedEventArgs<TGesture>> BehaviorChanged => _behaviorChanged;
	internal IObservable<GestureChangedEventArgs<TGesture>> GestureChanged => _gestureChanged;
	internal IObservable<HotKeyBinding<TGesture>> Disposed => _disposed;

	internal HotKeyBinding(BindingBehavior behavior, TGesture? gesture = null)
	{
		_behavior = behavior;
		Gesture = gesture;
	}

	internal void OnPressed() => Behavior.OnPressed();
	internal void OnReleased() => Behavior.OnReleased();

	private readonly Subject<BehaviorChangedEventArgs<TGesture>> _behaviorChanged = new();
	private readonly Subject<GestureChangedEventArgs<TGesture>> _gestureChanged = new();
	private readonly Subject<HotKeyBinding<TGesture>> _disposed = new();
	private TGesture? _gesture;
	private BindingBehavior _behavior;
}