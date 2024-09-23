using CommunityToolkit.Diagnostics;
using FluentAssertions;

namespace HotKeys.Tests;

public sealed class KeyManagerFilterTests
{
	[Fact]
	public void ShouldPassKeys()
	{
		ControlledKeyManager<char> keyManager = new();
		KeyManagerFilter<char> filter = new(keyManager);
		HashSet<char> pressedKeys = new();
		filter.KeyPressed.Subscribe(key => Guard.IsTrue(pressedKeys.Add(key)));
		filter.KeyReleased.Subscribe(key => Guard.IsTrue(pressedKeys.Remove(key)));
		keyManager.NotifyPressed('D');
		pressedKeys.Should().Contain('D');
		keyManager.NotifyPressed('K');
		pressedKeys.Should().Contain(['D', 'K']);
		keyManager.NotifyReleased('K');
		pressedKeys.Should().Contain('D');
		keyManager.NotifyReleased('D');
		pressedKeys.Should().BeEmpty();
	}

	[Fact]
	public void ShouldNotPassRepeatingSameKey()
	{
		ControlledKeyManager<char> keyManager = new();
		KeyManagerFilter<char> filter = new(keyManager);
		HashSet<char> pressedKeys = new();
		filter.KeyPressed.Subscribe(key => Guard.IsTrue(pressedKeys.Add(key)));
		filter.KeyReleased.Subscribe(key => Guard.IsTrue(pressedKeys.Remove(key)));
		for (int i = 0; i < 10; i++)
		{
			keyManager.NotifyPressed('D');
			pressedKeys.Should().Contain('D');
			pressedKeys.Should().HaveCount(1);
		}
	}

	[Fact]
	public void ShouldNotPassNotPressedKeyReleasing()
	{
		ControlledKeyManager<char> keyManager = new();
		KeyManagerFilter<char> filter = new(keyManager);
		HashSet<char> pressedKeys = new();
		filter.KeyPressed.Subscribe(key => Guard.IsTrue(pressedKeys.Add(key)));
		filter.KeyReleased.Subscribe(key => Guard.IsTrue(pressedKeys.Remove(key)));
		keyManager.NotifyReleased('D');
		pressedKeys.Should().BeEmpty();
	}

	[Fact]
	public void ShouldPassKeyPressAfterInvalidKeyRelease()
	{
		ControlledKeyManager<char> keyManager = new();
		KeyManagerFilter<char> filter = new(keyManager);
		HashSet<char> pressedKeys = new();
		filter.KeyPressed.Subscribe(key => Guard.IsTrue(pressedKeys.Add(key)));
		filter.KeyReleased.Subscribe(key => Guard.IsTrue(pressedKeys.Remove(key)));
		keyManager.NotifyReleased('D');
		keyManager.NotifyPressed('D');
		pressedKeys.Should().Contain('D');
	}
}