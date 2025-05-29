using CommunityToolkit.Diagnostics;

namespace HotKeys;

public sealed class BindingsManager : IDisposable
{
	public BindingsManager(IObservable<Gesture> gestureChanges)
	{
		_disposable = gestureChanges.Subscribe(OnGestureChanged);
	}

	public Binding CreateBinding(ContinuousHandler handler)
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
		Binding binding = new(this, handler);
		_bindings.Add(binding);
		return binding;
	}

	public void Dispose()
	{
		if (_isDisposed)
			return;
		_disposable.Dispose();
		foreach (var binding in _bindings)
			binding.SetDisposed();
		_isDisposed = true;
	}

	internal void UpdateBindingState(Binding binding)
	{
		binding.IsPressed = binding.IsEnabled && IsGesturePressed(binding);
	}

	internal void RemoveBinding(Binding binding)
	{
		var isRemoved = _bindings.Remove(binding);
		Guard.IsTrue(isRemoved);
	}

	private readonly List<Binding> _bindings = new();
	private readonly IDisposable _disposable;
	private Gesture _pressedGesture = Gesture.Empty;
	private bool _isDisposed;

	private void OnGestureChanged(Gesture gesture)
	{
		_pressedGesture = gesture;
		UpdateBindingsStates();
	}

	private void UpdateBindingsStates()
	{
		foreach (var binding in _bindings)
			UpdateBindingState(binding);
	}

	private bool IsGesturePressed(Binding binding)
	{
		return IsPressed(binding.Gesture);
	}

	private bool IsPressed(Gesture gesture)
	{
		if (gesture.IsEmpty)
			return false;
		var currentGestureKeys = _pressedGesture.Keys;
		var bindingKeys = gesture.Keys;
		return bindingKeys.Count <= currentGestureKeys.Count && bindingKeys.IsSubsetOf(currentGestureKeys);
	}
}