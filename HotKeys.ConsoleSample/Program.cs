using HotKeys;
using HotKeys.Gestures;
using HotKeys.SharpHook;
using SharpHook.Reactive;

SimpleReactiveGlobalHook hook = new();
KeyManagerFilter<FormattedSharpHookKeyCode> keyManager = new(new SharpHookKeyboardKeyManager(hook));
SharpHookMouseButtonsManager mouseButtonsManager = new(hook);
GestureManager gestureManager = new(new AggregateKeyManager([keyManager, mouseButtonsManager]));
gestureManager.CurrentGestureChanged.Subscribe(gesture => Console.WriteLine($"[GestureManager] Gesture: {gesture}"));

hook.RunAsync();
while (true)
	Console.ReadKey(true);