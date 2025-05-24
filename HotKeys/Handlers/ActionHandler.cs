namespace HotKeys.Handlers;

public sealed class ActionHandler : OnetimeHandler
{
	public ActionHandler(Action action)
	{
		_action = action;
	}

	public void Handle()
	{
		_action();
	}

	private readonly Action _action;
}