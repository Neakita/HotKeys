using Katter.HotKeys.Behaviours;

namespace Katter.HotKeys;

public sealed class HotKeyBindingsManager<TGesture> where TGesture : notnull
{
	public HotKeyBindingsManager(GestureManager<TGesture> gestureManager)
	{
		gestureManager.GesturePressed.Subscribe(OnPressed);
		gestureManager.GestureReleased.Subscribe(OnReleased);
	}

	public HotKeyBinding<TGesture> CreateBinding(string name, BindingBehaviour behaviour, TGesture gesture)
	{
		HotKeyBinding<TGesture> binding = new(name, gesture, behaviour, () => _bindings.Remove(gesture));
		_bindings.Add(gesture, binding);
		return binding;
	}

	public HotKeyBinding<TGesture> CreateBinding(string name, BindingBehaviour behaviour)
	{
		return new HotKeyBinding<TGesture>(name, behaviour);
	}

	public void SetGesture(HotKeyBinding<TGesture> binding, TGesture? gesture)
	{
		if (binding.IsDisposed)
			throw new ObjectDisposedException(binding.Name, "Binding is disposed");
		if (binding.Gesture != null)
			_bindings.Remove(binding.Gesture);
		binding.Gesture = gesture;
		if (gesture == null)
			binding.DisposeAction = null;
		else
		{
			_bindings.Add(gesture, binding);
			binding.DisposeAction = () => _bindings.Remove(gesture);
		}
	}

	private readonly Dictionary<TGesture, HotKeyBinding<TGesture>> _bindings = new();
	private readonly Dictionary<TGesture, HotKeyBinding<TGesture>> _pressedBindings = new();

	private void OnPressed(TGesture gesture)
	{
		if (!_bindings.TryGetValue(gesture, out var binding))
			return;
		_pressedBindings.Add(gesture, binding);
		binding.OnPressed();
	}

	private void OnReleased(TGesture gesture)
	{
		if (_pressedBindings.Remove(gesture, out var binding))
			binding.OnReleased();
	}
}