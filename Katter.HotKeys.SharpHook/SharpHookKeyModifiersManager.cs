using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reactive.Linq;
using SharpHook.Native;

namespace Katter.HotKeys.SharpHook;

public sealed class SharpHookKeyModifiersManager : KeyManager<KeyModifiers>
{
	private static readonly FrozenDictionary<KeyCode, KeyModifiers> KeyModifiers =
		ImmutableDictionary.CreateRange<KeyCode, KeyModifiers>([
			new(KeyCode.VcLeftAlt, SharpHook.KeyModifiers.LeftAlt),
			new(KeyCode.VcRightAlt, SharpHook.KeyModifiers.RightAlt),
			new(KeyCode.VcLeftControl, SharpHook.KeyModifiers.LeftControl),
			new(KeyCode.VcRightControl, SharpHook.KeyModifiers.RightControl),
			new(KeyCode.VcLeftShift, SharpHook.KeyModifiers.LeftShift),
			new(KeyCode.VcRightShift, SharpHook.KeyModifiers.RightShift),
			new(KeyCode.VcLeftMeta, SharpHook.KeyModifiers.LeftMeta),
			new(KeyCode.VcRightMeta, SharpHook.KeyModifiers.RightMeta),
		]).ToFrozenDictionary();

	public IObservable<KeyModifiers> KeyPressed => _keyCodeManager.KeyPressed.Select(AsModifier).Where(IsNotNone);
	public IObservable<KeyModifiers> KeyReleased => _keyCodeManager.KeyReleased.Select(AsModifier).Where(IsNotNone);

	public SharpHookKeyModifiersManager(KeyManager<KeyCode> keyCodeManager)
	{
		_keyCodeManager = keyCodeManager;
	}

	internal static bool IsModifier(KeyCode key) => KeyModifiers.ContainsKey(key); 

	private readonly KeyManager<KeyCode> _keyCodeManager;
	
	private static KeyModifiers AsModifier(KeyCode key) =>
		CollectionExtensions.GetValueOrDefault(KeyModifiers, key, SharpHook.KeyModifiers.None);

	private static bool IsNotNone(KeyModifiers modifiers) => modifiers != SharpHook.KeyModifiers.None;
}