using System.IO;
using System.Threading.Tasks;
using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class KeyBindSetting : Control
{
	[Export(PropertyHint.None, "must be a legal filename")] private string _name;
	private KeyBind _keyBind;
	[Export] private KeyBind _defaultKeyBind;

	[Export(PropertyHint.Dir)] private string _dir;

	[Export] private string _rebindKeyButtonTextPrefix;
	[Export] private string _rebindMouseButtonButtonTextPrefix;

	[Export] private string _rebindingKeyText;
	[Export] private string _rebindingMouseButtonText;
	
	[Export] private NextInputActionGetter _nextActionGetter;
	[Export] private Button _rebindKeyButton;
	[Export] private Button _rebindMouseButtonButton;
	[Export] private Label _nameLabel;
	
	private StringName _nameStringName;

	private const Key CancelRebindingKey = Key.Escape;
	
	public override void _Ready()
	{
		Load();
		_nameStringName = _name;
		_nameLabel.Text = _name;
	}

	private void Load()
	{
		string path = GetSavePath();
		string globalPath = ProjectSettings.GlobalizePath(path);

		if (!File.Exists(globalPath))
		{
			_keyBind = new(_defaultKeyBind);
			return;
		}
		
		_keyBind = ResourceLoader.Load<KeyBind>(path);
	}
	private string GetSavePath()
		=> _dir + _name + ".tres";
	private void Apply()
		=> _keyBind.TryApplying(_nameStringName);

	private void Save()
	{
		Directory.CreateDirectory(ProjectSettings.GlobalizePath(_dir));
		ResourceSaver.Save(_keyBind, GetSavePath());
	}

	private async Task RebindKeyInternal()
	{
		_rebindKeyButton.Text = _rebindingKeyText;
		
		InputEventKey key = await RebindKeyButCancelable();
		_keyBind.Key = key;
		Apply();
		
		string keyName = _keyBind.Key?.KeyLabel.ToString() ?? "none";
		_rebindKeyButton.Text = _rebindKeyButtonTextPrefix + keyName;
	}
	
	private async Task<InputEventKey> RebindKeyButCancelable()
	{
		InputEventKey key = await _nextActionGetter.GetNextKeyPress();
		
		return key.KeyLabel == Key.Escape ? null : key;
	}

	private async Task RebindMouseButtonInternal()
	{
		_rebindMouseButtonButton.Text = _rebindingMouseButtonText;
		
		InputEventMouseButton button = await GetNextMouseButtonButCancelable();
		_keyBind.MouseButton = button;
		Apply();

		string mouseButtonName = _keyBind.MouseButton?.ButtonIndex.ToString() ?? "none"; 
		_rebindMouseButtonButton.Text = _rebindMouseButtonButtonTextPrefix + mouseButtonName;
	}
	
	private async Task<InputEventMouseButton> GetNextMouseButtonButCancelable()
	{
		while (true)
		{
			Task<InputEventMouseButton> mouseTask = _nextActionGetter.GetNextMousePress();
			Task<InputEventKey> keyTask = _nextActionGetter.GetNextKeyPress();

			Task completed = await Task.WhenAny(mouseTask, keyTask);

			if (completed == mouseTask)
			{
				InputEventMouseButton button = await mouseTask;
				return button;
			}
			
			InputEventKey key = await keyTask;
			if (key.KeyLabel == CancelRebindingKey)
				return null;
		}
	}

	private void SetButtonsEnabled(bool enabled)
	{
		_rebindKeyButton.Disabled = !enabled;
		_rebindMouseButtonButton.Disabled = !enabled;
	}
	
	private void RebindKey()
		=> _ = RebindKeyInternal();
	private void RebindMouseButton()
		=> _ = RebindMouseButtonInternal();
}
