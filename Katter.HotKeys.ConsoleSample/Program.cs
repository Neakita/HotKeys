using Katter.HotKeys;
using Katter.HotKeys.Behaviours;
using Katter.HotKeys.SharpHook;
using SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

SimpleReactiveGlobalHook hook = new(GlobalHookType.Keyboard);
SharpHookKeyboardKeyManager keyManager = new(hook);
SharpHookKeyModifiersManager modifiersManager = new(keyManager);
SharpHookKeyGestureManager keyGestureManager = new(modifiersManager, keyManager);
HotKeyBindingsManager<KeyGesture> hotKeyBindingsManager = new(keyGestureManager);
KeyGesture gesture = new(KeyCode.VcQ, KeyModifiers.Control | KeyModifiers.Shift);
ActionTapBindingBehaviour tapBehaviour = new(() => Console.WriteLine($"Tapped {gesture}!"));
ActionHoldBindingBehaviour holdBehaviour = new(token =>
{
	var start = DateTime.UtcNow;
	while (!token.IsCancellationRequested)
	{
		Console.Clear();
		Console.WriteLine($"Hold {gesture} for {DateTime.UtcNow - start}!");
		Thread.Sleep(1);
	}
	Console.Clear();
	Console.WriteLine($"Hold {gesture} for {DateTime.UtcNow - start}!");
});
hotKeyBindingsManager.CreateBinding(tapBehaviour, gesture);
hotKeyBindingsManager.CreateBinding(holdBehaviour, gesture);
hook.Run();