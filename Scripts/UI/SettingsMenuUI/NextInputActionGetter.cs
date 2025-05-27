using System.Threading.Tasks;
using Godot;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class NextInputActionGetter : Node
{
	private TaskCompletionSource<InputEventKey> _keyTaskSource;
	private TaskCompletionSource<InputEventMouseButton> _mouseTaskSource;

	public async Task<InputEventMouseButton> GetNextMousePress()
		=> await GetNextMousePressInternal();

	public async Task<InputEventKey> GetNextKeyPress()
		=> await GetNextKeyPressInternal();

	public override void _Input(InputEvent @event)
	{
		if (_keyTaskSource is not null && @event is InputEventKey eventKey)
			HandleEventKey(eventKey);
		else if (_mouseTaskSource is not null && @event is InputEventMouseButton eventButton)
			HandleEventMouseButton(eventButton);
	}

	private void HandleEventKey(InputEventKey eventKey)
	{
		if (!eventKey.Pressed || eventKey.Echo)
			return;
		
		_keyTaskSource.SetResult(eventKey);
		_keyTaskSource = null;
	}

	private void HandleEventMouseButton(InputEventMouseButton eventButton)
	{
		if (!eventButton.Pressed)
			return;
		
		_mouseTaskSource.SetResult(eventButton);
		_mouseTaskSource = null;
	}

	private async Task<InputEventKey> GetNextKeyPressInternal()
	{
		_keyTaskSource = new();
		return await _keyTaskSource.Task;
	}
	
	private async Task<InputEventMouseButton> GetNextMousePressInternal()
	{
		_mouseTaskSource = new();
		return await _mouseTaskSource.Task;
	}
}