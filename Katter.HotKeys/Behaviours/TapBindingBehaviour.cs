using CommunityToolkit.Diagnostics;

namespace Katter.HotKeys.Behaviours;

public abstract class TapBindingBehaviour : BindingBehaviour
{
	public TimeSpan MaximumInterval { get; set; } = TimeSpan.FromMilliseconds(125);

	protected internal sealed override void OnPressed()
	{
		Guard.IsNull(_pressTime);
		_pressTime = DateTime.UtcNow;
	}

	protected internal sealed override void OnReleased()
	{
		Guard.IsNotNull(_pressTime);
		var interval = DateTime.UtcNow - _pressTime.Value;
		if (interval <= MaximumInterval)
			Task.Run(OnTap);
		_pressTime = null;
	}

	private DateTime? _pressTime;

	protected abstract void OnTap();
}