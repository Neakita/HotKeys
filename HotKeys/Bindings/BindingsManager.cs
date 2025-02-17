using System.Reactive.Disposables;
using HotKeys.Gestures;

namespace HotKeys.Bindings;

public sealed class BindingsManager : IDisposable
{
	public BindingsManager(IObservable<Gesture> observableGesture)
	{
		_disposable = observableGesture.Subscribe(OnGestureChanged);
	}

	public IDisposable Bind(Gesture gesture, ContinuousHandler handler)
	{
		Binding binding = new(gesture, handler);
		var isPressed = IsPressed(gesture);
		_stateManager.AddBinding(binding, isPressed);
		if (isPressed)
			handler.Begin();
		return Disposable.Create((_stateManager, binding), static tuple =>
		{
			var (stateManager, binding) = tuple;
			stateManager.RemoveBinding(binding, out var wasPressed);
			if (wasPressed)
				binding.Handler.End();
		});
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly BindingsStateManager _stateManager = new();
	private readonly IDisposable _disposable;
	private Gesture _currentGesture = Gesture.Empty;

	private void OnGestureChanged(Gesture currentGesture)
	{
		_currentGesture = currentGesture;

		var justPressedBindings = _stateManager.NotPressedBindings
			.TakeWhile(pair => pair.Key <= currentGesture.Keys.Count)
			.SelectMany(pair => pair.Value)
			.Where(IsPressed)
			.ToList();

		var justReleasedBindings = _stateManager
			.PressedBindings
			.WhereKeyNot(IsPressed)
			.ToList();

		foreach (var (gesture, bindings) in justReleasedBindings)
		{
			foreach (var binding in bindings)
				binding.Handler.End();
			_stateManager.SetNotPressed(gesture);
		}

		foreach (var binding in justPressedBindings)
		{
			binding.Handler.Begin();
			_stateManager.SetPressed(binding);
		}
	}

	private bool IsPressed(Binding binding)
	{
		return IsPressed(binding.Gesture);
	}

	private bool IsPressed(Gesture gesture)
	{
		var currentGestureKeys = _currentGesture.Keys;
		var bindingKeys = gesture.Keys;
		return bindingKeys.Count <= currentGestureKeys.Count && bindingKeys.IsSubsetOf(currentGestureKeys);
	}
}