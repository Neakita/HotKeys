namespace HotKeys;

public enum InputTypes : byte
{
	/// <summary>
	/// Triggers only when a button is pushed down and released quickly.
	/// </summary>
	Tap = 1 << 0,

	/// <summary>
	/// Triggers on the second press if you quickly push down the same button twice.
	/// </summary>
	DoubleTap = 1 << 1,

	/// <summary>
	/// Triggers as soon as a button is pushed down.
	/// </summary>
	Press = 1 << 2,

	/// <summary>
	/// Triggers after a button has been pushed down for a set amount of time.
	/// </summary>
	LongPress = 1 << 3,

	/// <summary>
	/// Triggers as soon as a button is released.
	/// </summary>
	Release = 1 << 4,

	/// <summary>
	/// Triggers continuously while a button is being pushed down.
	/// </summary>
	Hold = 1 << 5,

	/// <summary>
	/// After a button has been pushed down for a set amount of time, this starts triggering continuously.
	/// </summary>
	LongHold = 1 << 6,

	AllOneTime = Tap | DoubleTap | Press | LongPress | Release,
	AllContinuous = Hold | LongHold,
	All = AllOneTime | AllContinuous
}