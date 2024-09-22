﻿using Katter.HotKeys.SharpHook;
using SharpHook;
using SharpHook.Reactive;

SimpleReactiveGlobalHook hook = new(GlobalHookType.Keyboard);
SharpHookKeyboardKeyManager keyManager = new(hook);
keyManager.KeyPressed.Subscribe(key => Console.WriteLine($"[KeyManager] Pressed {key}"));
keyManager.KeyReleased.Subscribe(key => Console.WriteLine($"[KeyManager] Released {key}"));
hook.Run();