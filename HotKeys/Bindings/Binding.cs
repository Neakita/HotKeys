using HotKeys.Gestures;

namespace HotKeys.Bindings;

internal sealed class Binding
{
	public Gesture Gesture { get; }
	public ContinuousHandler Handler { get; }

	public Binding(Gesture gesture, ContinuousHandler handler)
	{
		Gesture = gesture;
		Handler = handler;
	}
}