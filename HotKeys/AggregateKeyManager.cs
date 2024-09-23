using System.Reactive.Linq;

namespace HotKeys;

public sealed class AggregateKeyManager : KeyManager
{
	public IObservable<object> KeyPressed { get; }
	public IObservable<object> KeyReleased { get; }

	public AggregateKeyManager(IReadOnlyCollection<KeyManager> keyManagers)
	{
		KeyPressed = keyManagers.Select(keyManager => keyManager.KeyPressed).Merge();
		KeyReleased = keyManagers.Select(keyManager => keyManager.KeyReleased).Merge();
	}
}