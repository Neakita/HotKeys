using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using Katter.HotKeys.Behaviors;

namespace Katter.HotKeys;

public sealed class HotKeyBindingsManager<TGesture> where TGesture : class
{
	public HotKeyBindingsManager(GestureManager<TGesture> gestureManager)
	{
		gestureManager.GesturePressed.Subscribe(OnPressed);
		gestureManager.GestureReleased.Subscribe(OnReleased);
	}

	public HotKeyBinding<TGesture> CreateBinding(BindingBehavior behavior, TGesture? gesture = null)
	{
		HotKeyBinding<TGesture> binding = new(behavior, gesture);
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

	private void OnBindingGestureChanged(GestureChangedEventArgs<TGesture> args)
	{
		if (args.OldGesture != null)
		{
			bool isRemoved = _bindings[args.OldGesture].Remove(args.Sender);
			Guard.IsTrue(isRemoved);
		}
		if (args.NewGesture != null)
			GetOrCreateGesturesList(args.NewGesture).Add(args.Sender);
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