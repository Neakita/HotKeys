using System.Reactive.Linq;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

namespace HotKeys.SharpHook;

public class SharpHookKeyboardKeyManager : FilteredKeyManager<KeyCode>
{
	public SharpHookKeyboardKeyManager(IReactiveGlobalHook hook) : base(
		hook.KeyPressed.Select(TransformArgs),
		hook.KeyReleased.Select(TransformArgs))
	{
	}

	private static KeyCode TransformArgs(KeyboardHookEventArgs args)
	{
		return args.Data.KeyCode;
	}
}