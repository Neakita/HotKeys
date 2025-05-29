namespace HotKeys;

public sealed class Binding : IDisposable
{
	public ContinuousHandler Handler
	{
		get;
		set
		{
			ThrowIfDisposed();
			if (IsPressed)
			{
				field.End();
				value.Begin();
			}
			field = value;
		}
	}

	public Gesture Gesture
	{
		get;
		set
		{
			ThrowIfDisposed();
			field = value;
			// the state of the new gesture may not match the state of the previous gesture.
			_manager.UpdateBindingState(this);
		}
	}

	public bool IsPressed
	{
		get;
		internal set
		{
			ThrowIfDisposed();
			if (field == value)
				return;
			field = value;
			if (value)
				Handler.Begin();
			else
				Handler.End();
		}
	}

	public bool IsEnabled
	{
		get;
		set
		{
			ThrowIfDisposed();
			if (field == value)
				return;
			field = value;
			_manager.UpdateBindingState(this);
		}
	} = true;

	public void Dispose()
	{
		if (_isDisposed)
			return;
		_manager.RemoveBinding(this);
		SetDisposed();
	}

	internal Binding(BindingsManager manager, ContinuousHandler handler)
	{
		_manager = manager;
		Handler = handler;
		Gesture = Gesture.Empty;
	}

	internal void SetDisposed()
	{
		_isDisposed = true;
		IsPressed = false;
	}

	private readonly BindingsManager _manager;
	private bool _isDisposed;

	private void ThrowIfDisposed()
	{
		ObjectDisposedException.ThrowIf(_isDisposed, this);
	}
}