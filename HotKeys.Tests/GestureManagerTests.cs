using FluentAssertions;
using HotKeys.Gestures;

namespace HotKeys.Tests;

public sealed class GestureManagerTests
{
	[Fact]
	public void CurrentGestureChangedShouldBeObservedOnce()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		int changesCount = 0;
		using var subscription = gestureManager.CurrentGestureChanged.Subscribe(_ => changesCount++);
		keyManager.NotifyPressed('D');
		changesCount.Should().Be(1);
	}

	[Fact]
	public void GestureShouldHaveSingleProvidedKey()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		Gesture? observedGesture = null;
		using var subscription = gestureManager.CurrentGestureChanged.Subscribe(gesture => observedGesture = gesture);
		keyManager.NotifyPressed('D');
		observedGesture.Should().NotBeNull();
		observedGesture!.Keys.Single().Should().Be('D');
	}

	[Fact]
	public void GestureShouldBeEmptyWhenReleasingKey()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		Gesture? observedGesture = null;
		using var subscription = gestureManager.CurrentGestureChanged.Subscribe(gesture => observedGesture = gesture);
		keyManager.NotifyPressed('D');
		keyManager.NotifyReleased('D');
		observedGesture.Should().NotBeNull();
		observedGesture!.Keys.Should().BeEmpty();
	}

	[Fact]
	public void CurrentGestureChangedShouldBeObservedTwiceWhenReleasing()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		int changesCount = 0;
		using var subscription = gestureManager.CurrentGestureChanged.Subscribe(_ => changesCount++);
		keyManager.NotifyPressed('D');
		keyManager.NotifyReleased('D');
		changesCount.Should().Be(2);
	}

	[Fact]
	public void ShouldThrowWhenTryingToReleaseNotPressedKey()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		Assert.ThrowsAny<Exception>(() => keyManager.NotifyReleased('D'));
	}

	[Fact]
	public void ShouldThrowWhenTryingToPressSameKeyTwice()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		keyManager.NotifyPressed('D');
		Assert.ThrowsAny<Exception>(() => keyManager.NotifyPressed('D'));
	}

	[Fact]
	public void ShouldHaveTwoKeysPressed()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		Gesture? observedGesture = null;
		using var subscription = gestureManager.CurrentGestureChanged.Subscribe(gesture => observedGesture = gesture);
		keyManager.NotifyPressed('D');
		keyManager.NotifyPressed('F');
		observedGesture.Should().NotBeNull();
		observedGesture!.Keys.Should().Contain(['D', 'F']);
	}

	[Fact]
	public void KeyShouldBePressedWhenOtherKeyIsReleased()
	{
		using ControlledKeyManager<char> keyManager = new();
		using GestureManager gestureManager = new(keyManager);
		Gesture? observedGesture = null;
		using var subscription = gestureManager.CurrentGestureChanged.Subscribe(gesture => observedGesture = gesture);
		keyManager.NotifyPressed('D');
		keyManager.NotifyPressed('F');
		keyManager.NotifyReleased('D');
		observedGesture.Should().NotBeNull();
		observedGesture!.Keys.Single().Should().Be('F');
	}
}