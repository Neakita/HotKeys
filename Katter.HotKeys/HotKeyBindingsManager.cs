using CommunityToolkit.Diagnostics;
using Katter.HotKeys.Behaviours;

namespace Katter.HotKeys;

public sealed class HotKeyBindingsManager<TGesture> where TGesture : class
{
	public HotKeyBindingsManager(GestureManager<TGesture> gestureManager)
	{
		gestureManager.GesturePressed.Subscribe(OnPressed);
		gestureManager.GestureReleased.Subscribe(OnReleased);
	}

	public HotKeyBinding<TGesture> CreateBinding(string name, BindingBehaviour behaviour, TGesture? gesture = null)
	{
		HotKeyBinding<TGesture> binding = new(name, behaviour, gesture);
		if (gesture != null)
			_bindings.Add(gesture, binding);
		binding.GestureChanged.Subscribe(OnBindingGestureChanged);
		binding.Disposed.Subscribe(OnBindingDisposed);
		return binding;
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

	private void OnBindingDisposed(HotKeyBinding<TGesture> disposedBinding)
	{
		if (disposedBinding.Gesture != null)
			_bindings.Remove(disposedBinding.Gesture);
	}

	private void OnBindingGestureChanged((HotKeyBinding<TGesture> sender, TGesture? oldGesture, TGesture? newGesture) t)
	{
		if (t.oldGesture != null)
		{
			bool isRemoved = _bindings.Remove(t.oldGesture, out var removedBinding);
			Guard.IsTrue(isRemoved);
			Guard.IsReferenceEqualTo(removedBinding!, t.sender);
		}
		if (t.newGesture != null)
			_bindings.Add(t.newGesture, t.sender);
	}
}