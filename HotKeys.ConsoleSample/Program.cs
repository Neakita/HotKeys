using HotKeys;
using HotKeys.Bindings;
using HotKeys.Gestures;
using HotKeys.SharpHook;
using SharpHook.Native;
using SharpHook.Reactive;

SimpleReactiveGlobalHook hook = new();
KeyManagerFilter<FormattedKeyCode> keyManager = new(new SharpHookKeyboardKeyManager(hook));
SharpHookMouseButtonsManager mouseButtonsManager = new(hook);
GestureManager gestureManager = new(new AggregateKeyManager([keyManager, mouseButtonsManager]));
gestureManager.CurrentGestureChanged.Subscribe(gesture => Console.WriteLine($"[GestureManager] Gesture: {gesture}"));

BindingsManager bindingsManager = new(gestureManager);
var binding = bindingsManager.CreateBinding(() => Console.WriteLine("[BindingsManager] Action!"), InputTypes.Tap);
var continuousBinding = bindingsManager.CreateBinding(context =>
{
	var start = DateTime.UtcNow;
	context.WaitForElimination();
	Console.WriteLine($"[BindingsManager] Action for {(DateTime.UtcNow - start).TotalSeconds} seconds!");
}, InputTypes.LongHold);
var gesture = new Gesture([new FormattedKeyCode(KeyCode.VcQ)]);
bindingsManager.SetGesture(binding, gesture);
bindingsManager.SetGesture(continuousBinding, gesture);

hook.RunAsync();
while (true)
	Console.ReadKey(true);