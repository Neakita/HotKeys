using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;
using HotKeys.Gestures;

namespace HotKeys.Bindings;

public sealed class BindingsManager : IDisposable
{
	public BindingsManager(GestureManager gestureManager)
	{
		_disposable = gestureManager.CurrentGestureChanged.Subscribe(OnGestureChanged);
	}

	public Binding CreateBinding(
		Action action,
		InputTypes availableInputTypes = InputTypes.AllOneTime,
		InputTypes initialInputType = InputTypes.Press)
	{
		return CreateBinding(new PlainActionRunner(action), availableInputTypes, initialInputType);
	}

	public Binding CreateBinding(
		Action<CancellationToken> action,
		InputTypes availableInputTypes = InputTypes.AllContinuous,
		InputTypes initialInputType = InputTypes.Hold)
	{
		return CreateBinding(new CancellableActionRunner(action), availableInputTypes, initialInputType);
	}

	public Binding CreateBinding(
		Action<Task> action,
		InputTypes availableInputTypes = InputTypes.AllContinuous,
		InputTypes initialInputType = InputTypes.Hold)
	{
		return CreateBinding(new TaskActionRunner(action), availableInputTypes, initialInputType);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private Binding CreateBinding(
		ActionRunner actionRunner,
		InputTypes availableInputTypes,
		InputTypes initialInputType)
	{
		Binding binding = new(actionRunner, availableInputTypes, initialInputType, RemoveBinding);
		Guard.IsTrue(_bindings.Add(binding));
		Guard.IsTrue(_notPressedBindings.Add(binding));
		return binding;
	}

	private readonly HashSet<Binding> _bindings = new();
	private readonly HashSet<Binding> _notPressedBindings = new();
	private readonly Dictionary<Gesture, HashSet<Binding>> _pressedBindings = new();
	private readonly IDisposable _disposable;

	private void RemoveBinding(Binding binding)
	{
		Guard.IsTrue(_bindings.Remove(binding));
	}

	private void OnGestureChanged(Gesture currentGesture)
	{
		foreach (var (gesture, bindings) in _pressedBindings)
		foreach (var binding in bindings)
		{
			if (gesture.Keys.IsSubsetOf(currentGesture.Keys))
				continue;
			Guard.IsTrue(bindings.Remove(binding));
			if (bindings.Count == 0)
				Guard.IsTrue(_pressedBindings.Remove(gesture));
			Guard.IsTrue(_notPressedBindings.Add(binding));
			binding.Behavior.OnReleased();
		}

		foreach (var binding in _notPressedBindings)
		{
			if (binding.Gesture == null)
				continue;
			if (binding.Gesture.Keys.Count > currentGesture.Keys.Count)
				continue;
			if (!binding.Gesture.Keys.IsSubsetOf(currentGesture.Keys))
				continue;
			Guard.IsTrue(_notPressedBindings.Remove(binding));
			if (_pressedBindings.TryGetValue(currentGesture, out var bindings))
				Guard.IsTrue(bindings.Add(binding));
			else
				_pressedBindings.Add(currentGesture, [binding]);
			binding.Behavior.OnPressed();
		}
	}
}