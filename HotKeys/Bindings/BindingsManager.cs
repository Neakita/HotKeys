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
		Action<ActionContext> action,
		InputTypes availableInputTypes = InputTypes.AllOneTime,
		InputTypes initialInputType = InputTypes.Press)
	{
		ContextActionRunner actionRunner = new(action);
		var binding = CreateBinding(actionRunner, availableInputTypes, initialInputType);
		actionRunner.Initialize(binding);
		return binding;
	}

	public void SetGesture(Binding binding, Gesture? gesture)
	{
		if (binding.Gesture != null)
		{
			if (_bindingsStateManager.IsPressed(binding))
				binding.Behavior.OnReleased();
			_bindingsStateManager.RemoveBinding(binding);
		}
		binding.Gesture = gesture;
		if (gesture != null)
		{
			var shouldBePressed = ShouldBePressed(binding);
			_bindingsStateManager.AddBinding(binding, shouldBePressed);
			if (shouldBePressed)
				binding.Behavior.OnPressed();
		}
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly BindingsStateManager _bindingsStateManager = new();
	private readonly IDisposable _disposable;
	private Gesture _currentGesture = Gesture.Empty;

	private Binding CreateBinding(
		ActionRunner actionRunner,
		InputTypes availableInputTypes,
		InputTypes initialInputType)
	{
		Binding binding = new(actionRunner, availableInputTypes, initialInputType, _bindingsStateManager.RemoveBinding);
		return binding;
	}

	private void OnGestureChanged(Gesture currentGesture)
	{
		_currentGesture = currentGesture;

		var justPressedBindings = _bindingsStateManager.NotPressedBindings
			.TakeWhile(pair => pair.Key <= currentGesture.Keys.Count)
			.SelectMany(pair => pair.Value)
			.Where(ShouldBePressed)
			.ToList();

		var justReleasedBindings = _bindingsStateManager
			.PressedBindings
			.WhereKeyNot(ShouldBePressed)
			.ToList();

		foreach (var (gesture, bindings) in justReleasedBindings)
		{
			foreach (var binding in bindings)
				binding.Behavior.OnReleased();
			_bindingsStateManager.SetNotPressed(gesture);
		}

		foreach (var binding in justPressedBindings)
		{
			binding.Behavior.OnPressed();
			_bindingsStateManager.SetPressed(binding);
		}
	}

	private bool ShouldBePressed(Binding binding)
	{
		Guard.IsNotNull(binding.Gesture);
		return ShouldBePressed(binding.Gesture);
	}

	private bool ShouldBePressed(Gesture gesture)
	{
		var currentGestureKeys = _currentGesture.Keys;
		var bindingKeys = gesture.Keys;
		return bindingKeys.Count <= currentGestureKeys.Count && bindingKeys.IsSubsetOf(currentGestureKeys);
	}
}