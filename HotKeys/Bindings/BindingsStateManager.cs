using CommunityToolkit.Diagnostics;
using HotKeys.Gestures;

namespace HotKeys.Bindings;

internal sealed class BindingsStateManager
{
	public SortedList<byte, HashSet<Binding>> NotPressedBindings { get; } = new();
	public HashSet<Binding> PressedBindings { get; } = new();

	public void AddBinding(Binding binding, bool pressed)
	{
		Guard.IsNotNull(binding.Gesture);
		var set = pressed ? PressedBindings : GetNotPressedBindingsSet(binding.Gesture);
		Guard.IsTrue(set.Add(binding));
	}

	public void SetPressed(Binding binding)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var notPressedBindings = NotPressedBindings[(byte)gesture.Keys.Count];
		Guard.IsTrue(notPressedBindings.Remove(binding));
		Guard.IsTrue(PressedBindings.Add(binding));
	}

	public void SetNotPressed(Binding binding)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var notPressedBindings = NotPressedBindings[(byte)gesture.Keys.Count];
		Guard.IsTrue(PressedBindings.Remove(binding));
		Guard.IsTrue(notPressedBindings.Add(binding));
	}

	public bool IsPressed(Binding binding)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		if (PressedBindings.Contains(binding))
			return true;
		var notPressedBindings = NotPressedBindings[(byte)gesture.Keys.Count];
		Guard.IsTrue(notPressedBindings.Contains(binding));
		return false;
	}

	public void RemoveBinding(Binding binding)
	{
		var gesture = binding.Gesture;
		Guard.IsNotNull(gesture);
		var notPressedBindings = NotPressedBindings[(byte)gesture.Keys.Count];
		Guard.IsTrue(notPressedBindings.Remove(binding) || PressedBindings.Remove(binding));
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