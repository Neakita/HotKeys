using System.Reactive.Linq;

namespace HotKeys;

public interface KeyManager<out T> : KeyManager where T : notnull
{
	new IObservable<T> KeyPressed { get; }
	new IObservable<T> KeyReleased { get; }

	IObservable<object> KeyManager.KeyPressed => KeyPressed.Select(key => (object)key);
	IObservable<object> KeyManager.KeyReleased => KeyReleased.Select(key => (object)key);
}