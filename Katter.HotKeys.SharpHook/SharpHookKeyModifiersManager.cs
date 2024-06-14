using System.Collections.Immutable;
using System.Reactive.Linq;
using SharpHook.Native;

namespace Katter.HotKeys.SharpHook;

public sealed class SharpHookKeyModifiersManager : KeyManager<KeyModifiers>
{
	private static readonly ImmutableDictionary<KeyCode, KeyModifiers> KeyModifiers =
		ImmutableDictionary.CreateRange<KeyCode, KeyModifiers>([
			new(KeyCode.VcLeftAlt, SharpHook.KeyModifiers.Alt),
			new(KeyCode.VcRightAlt, SharpHook.KeyModifiers.Alt),
			new(KeyCode.VcLeftControl, SharpHook.KeyModifiers.Control),
			new(KeyCode.VcRightControl, SharpHook.KeyModifiers.Control),
			new(KeyCode.VcLeftShift, SharpHook.KeyModifiers.Shift),
			new(KeyCode.VcRightShift, SharpHook.KeyModifiers.Shift),
			new(KeyCode.VcLeftMeta, SharpHook.KeyModifiers.Meta),
			new(KeyCode.VcRightMeta, SharpHook.KeyModifiers.Meta)
		]);

	public IObservable<KeyModifiers> KeyPressed => _keyCodeManager.KeyPressed.Where(IsModifier).Select(AsModifier);
	public IObservable<KeyModifiers> KeyReleased => _keyCodeManager.KeyReleased.Where(IsModifier).Select(AsModifier);

	public SharpHookKeyModifiersManager(KeyManager<KeyCode> keyCodeManager)
	{
		_keyCodeManager = keyCodeManager;
	}

	internal static bool IsModifier(KeyCode key) => KeyModifiers.ContainsKey(key); 

	private readonly KeyManager<KeyCode> _keyCodeManager;
	
	private static KeyModifiers AsModifier(KeyCode key)
	{
		return KeyModifiers[key];
	}
}