using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reactive.Linq;
using SharpHook.Native;

namespace Katter.HotKeys.SharpHook;

public sealed class SharpHookKeyModifiersManager : KeyManager<KeyModifiers>
{
	private static readonly FrozenDictionary<KeyCode, KeyModifiers> KeyModifiers =
		ImmutableDictionary.CreateRange<KeyCode, KeyModifiers>([
			new(KeyCode.VcLeftAlt, HotKeys.KeyModifiers.LeftAlt),
			new(KeyCode.VcRightAlt, HotKeys.KeyModifiers.RightAlt),
			new(KeyCode.VcLeftControl, HotKeys.KeyModifiers.LeftControl),
			new(KeyCode.VcRightControl, HotKeys.KeyModifiers.RightControl),
			new(KeyCode.VcLeftShift, HotKeys.KeyModifiers.LeftShift),
			new(KeyCode.VcRightShift, HotKeys.KeyModifiers.RightShift),
			new(KeyCode.VcLeftMeta, HotKeys.KeyModifiers.LeftMeta),
			new(KeyCode.VcRightMeta, HotKeys.KeyModifiers.RightMeta),
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
		CollectionExtensions.GetValueOrDefault(KeyModifiers, key, HotKeys.KeyModifiers.None);

	private static bool IsNotNone(KeyModifiers modifiers) => modifiers != HotKeys.KeyModifiers.None;
}