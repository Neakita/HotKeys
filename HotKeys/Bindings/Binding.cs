using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Diagnostics;
using HotKeys.ActionRunners;
using HotKeys.Behaviors;
using HotKeys.Gestures;

namespace HotKeys.Bindings;

public sealed class Binding : IDisposable
{
	public InputTypes AvailableInputTypes { get; }

	public InputTypes InputType
	{
		get => _inputType;
		[MemberNotNull(nameof(Behavior))] set
		{
			_inputType = value;
			Guard.IsTrue(AvailableInputTypes.HasFlag(value));
			Behavior = Behavior.Create(value, _actionRunner);
		}
	}

	public Gesture? Gesture
	{
		get => _gesture;
		internal set
		{
			if (value != null)
				Guard.IsFalse(value.Keys.IsEmpty);
			_gesture = value;
		}
	}

	public void Dispose()
	{
		_disposeAction(this);
	}
	
	internal Behavior Behavior { get; private set; }

	internal Binding(
		ActionRunner actionRunner,
		InputTypes availableInputTypes,
		InputTypes initialInputType,
		Action<Binding> disposeAction)
	{
		AvailableInputTypes = availableInputTypes;
		_actionRunner = actionRunner;
		InputType = initialInputType;
		_disposeAction = disposeAction;
	}

	private readonly ActionRunner _actionRunner;
	private readonly Action<Binding> _disposeAction;
	private InputTypes _inputType;
	private Gesture? _gesture;
}