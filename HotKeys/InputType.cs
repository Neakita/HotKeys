namespace HotKeys;

public enum InputType
{
	/// <summary>
	/// Triggers only when a button is pushed down and released quickly.
	/// </summary>
	Tap,
	/// <summary>
	/// Triggers on the second press if you quickly push down the same button twice.
	/// </summary>
	DoubleTap,
	/// <summary>
	/// Triggers as soon as a button is pushed down.
	/// </summary>
	Press,
	/// <summary>
	/// Triggers after a button has been pushed down for a set amount of time.
	/// </summary>
	LongPress,
	/// <summary>
	/// Triggers as soon as a button is released.
	/// </summary>
	Release,
	/// <summary>
	/// Triggers continuously while a button is being pushed down.
	/// </summary>
	Hold,
	/// <summary>
	/// After a button has been pushed down for a set amount of time, this starts triggering continuously.
	/// </summary>
	LongHold
}