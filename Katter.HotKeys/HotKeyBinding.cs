using CommunityToolkit.Diagnostics;
using Katter.HotKeys.Behaviours;

namespace Katter.HotKeys;

public sealed class HotKeyBinding<TGesture> : IDisposable
{
	public string Name { get; }

	public TGesture? Gesture
	{
		get => _gesture;
		internal set
		{
			Guard.IsFalse(IsDisposed);
			_gesture = value;
		}
	}

	internal Action? DisposeAction
	{
		get => _disposeAction;
		set
		{
			Guard.IsFalse(IsDisposed);
			_disposeAction = value;
		}
	}

	internal bool IsDisposed { get; private set; }

	public void Dispose()
	{
		if (IsDisposed)
			return;
		IsDisposed = true;
		DisposeAction?.Invoke();
	}

	internal HotKeyBinding(string name, BindingBehaviour behaviour)
	{
		_behaviour = behaviour;
		Name = name;
	}

	internal HotKeyBinding(string name, TGesture gesture, BindingBehaviour behaviour, Action disposeAction)
	{
		_behaviour = behaviour;
		Name = name;
		Gesture = gesture;
		DisposeAction = disposeAction;
	}

	internal void OnPressed() => _behaviour.OnPressed();
	internal void OnReleased() => _behaviour.OnReleased();

	private readonly BindingBehaviour _behaviour;
	private TGesture? _gesture;
	private Action? _disposeAction;
}