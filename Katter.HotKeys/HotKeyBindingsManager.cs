using System.Collections.Immutable;
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
			GetOrCreateGesturesList(gesture).Add(binding);
		binding.GestureChanged.Subscribe(OnBindingGestureChanged);
		binding.Disposed.Subscribe(OnBindingDisposed);
		return binding;
	}

	private readonly Dictionary<TGesture, List<HotKeyBinding<TGesture>>> _bindings = new();
	private readonly Dictionary<TGesture, ImmutableArray<HotKeyBinding<TGesture>>> _pressedBindings = new();

	private void OnPressed(TGesture gesture)
	{
		if (!_bindings.TryGetValue(gesture, out var bindings))
			return;
		var pressedBindings = bindings.ToImmutableArray();
		_pressedBindings.Add(gesture, pressedBindings);
		foreach (var binding in pressedBindings)
			binding.OnPressed();
	}

	private void OnReleased(TGesture gesture)
	{
		var isRemoved = _pressedBindings.Remove(gesture, out var bindings);
		Guard.IsTrue(isRemoved);
		foreach (var binding in bindings)
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
			bool isRemoved = _bindings[t.oldGesture].Remove(t.sender);
			Guard.IsTrue(isRemoved);
		}
		if (t.newGesture != null)
			GetOrCreateGesturesList(t.newGesture).Add(t.sender);
	}

	private List<HotKeyBinding<TGesture>> GetOrCreateGesturesList(TGesture gesture)
	{
		if (_bindings.TryGetValue(gesture, out var existingList))
			return existingList;
		List<HotKeyBinding<TGesture>> newList = new();
		_bindings.Add(gesture, newList);
		return newList;
	}
}