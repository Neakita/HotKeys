using CommunityToolkit.Diagnostics;
using FluentAssertions;

namespace HotKeys.Tests;

public sealed class AggregateKeyManagerTests
{
	[Fact]
	public void ShouldMergeAllEvents()
	{
		using ControlledKeyManager<char> firstKeyManager = new();
		using ControlledKeyManager<char> secondKeyManager = new();
		AggregateKeyManager aggregateKeyManager = new([firstKeyManager, secondKeyManager]);
		HashSet<object> pressedKeys = new();
		aggregateKeyManager.KeyPressed.Subscribe(key => Guard.IsTrue(pressedKeys.Add(key)));
		aggregateKeyManager.KeyReleased.Subscribe(key => Guard.IsTrue(pressedKeys.Remove(key)));
		firstKeyManager.NotifyPressed('D');
		pressedKeys.Should().Contain('D');
		secondKeyManager.NotifyPressed('F');
		pressedKeys.Should().Contain(['D', 'F']);
		Assert.ThrowsAny<Exception>(() => firstKeyManager.NotifyPressed('D'));
		secondKeyManager.NotifyReleased('F');
		pressedKeys.Should().Contain('D');
		firstKeyManager.NotifyReleased('D');
		pressedKeys.Should().BeEmpty();
		Assert.ThrowsAny<Exception>(() => secondKeyManager.NotifyReleased('F'));
	}
}