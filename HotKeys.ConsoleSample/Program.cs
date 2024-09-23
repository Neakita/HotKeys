using HotKeys;
using HotKeys.SharpHook;
using SharpHook.Reactive;

SimpleReactiveGlobalHook hook = new();
SharpHookKeyboardKeyManager keyManager = new(hook);
keyManager.KeyPressed.Subscribe(key => Console.WriteLine($"[KeyManager] Pressed {key}"));
keyManager.KeyReleased.Subscribe(key => Console.WriteLine($"[KeyManager] Released {key}"));
SharpHookMouseButtonsManager mouseButtonsManager = new(hook);
mouseButtonsManager.KeyPressed.Subscribe(button => Console.WriteLine($"[MouseButtonsManager] Pressed {button}"));
mouseButtonsManager.KeyReleased.Subscribe(button => Console.WriteLine($"[MouseButtonsManager] Released {button}"));

GestureManager gestureManager = new(new AggregateKeyManager([keyManager, mouseButtonsManager]));
gestureManager.CurrentGestureChanged.Subscribe(gesture => Console.WriteLine($"[GestureManager] Gesture: {gesture}"));

hook.Run();
while (true)
	Console.ReadKey(true);