using CommunityToolkit.Diagnostics;
using HotKeys.Gestures;

namespace HotKeys.Bindings;

internal sealed class BindingsStateManager
{
	public SortedList<byte, HashSet<Binding>> NotPressedBindings { get; } = new();
	public Dictionary<Gesture, HashSet<Binding>> PressedBindings { get; } = new();

	public void AddBinding(Binding binding, bool pressed)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var set = pressed ? GetPressedBindingsSet(gesture) : GetNotPressedBindingsSet(gesture);
		Guard.IsTrue(set.Add(binding));
	}

	public void SetPressed(Binding binding)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var notPressedBindings = GetNotPressedBindingsSet(gesture);
		var pressedBindings = GetPressedBindingsSet(gesture);
		Guard.IsTrue(notPressedBindings.Remove(binding));
		Guard.IsTrue(pressedBindings.Add(binding));
	}

	public void SetNotPressed(Binding binding)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var notPressedBindings = GetNotPressedBindingsSet(gesture);
		var pressedBindings = GetPressedBindingsSet(gesture);
		Guard.IsTrue(pressedBindings.Remove(binding));
		Guard.IsTrue(notPressedBindings.Add(binding));
	}

	public void SetNotPressed(Gesture gesture)
	{
		Guard.IsNotNull(gesture);
		var notPressedBindings = GetNotPressedBindingsSet(gesture);
		var pressedBindings = GetPressedBindingsSet(gesture);
		Guard.IsTrue(pressedBindings.All(notPressedBindings.Add));
		pressedBindings.Clear();
	}

	public bool IsPressed(Binding binding)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var pressedBindings = GetPressedBindingsSet(gesture);
		var notPressedBindings = GetNotPressedBindingsSet(gesture);
		var pressedBindingsContains = pressedBindings.Contains(binding);
		var notPressedBindingsContains = notPressedBindings.Contains(binding);
		Guard.IsTrue(pressedBindingsContains ^ notPressedBindingsContains);
		return pressedBindingsContains;
	}

	public void RemoveBinding(Binding binding, out bool wasPressed)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var notPressedBindings = GetNotPressedBindingsSet(gesture);
		var pressedBindings = GetPressedBindingsSet(gesture);
		var removedFromPressed = pressedBindings.Remove(binding);
		Guard.IsTrue(removedFromPressed || notPressedBindings.Remove(binding));
		wasPressed = removedFromPressed;
	}

	private HashSet<Binding> GetPressedBindingsSet(Gesture gesture)
	{
		if (PressedBindings.TryGetValue(gesture, out var set))
			return set;
		set = new HashSet<Binding>();
		PressedBindings.Add(gesture, set);
		return set;
	}

	private HashSet<Binding> GetNotPressedBindingsSet(Gesture gesture)
	{
		var keysCount = (byte)gesture.Keys.Count;
		if (NotPressedBindings.TryGetValue(keysCount, out var set))
			return set;
		set = new HashSet<Binding>();
		NotPressedBindings.Add(keysCount, set);
		return set;
	}
}