using Godot;

namespace ULTRAmiami.UI.SettingsMenuUI;

[GlobalClass]
public partial class VolumeSetting : Resource
{
	[Export] private string _saveName;
	
	[Export] private int _bus;
	[Export] public float Volume = 1;
	
	private const string SavePath = "user://Settings/Volumes/";

	public void Apply()
	{
		AudioServer.SetBusVolumeLinear(_bus, Volume);
	}

	public void Save()
	{
		System.IO.Directory.CreateDirectory(ProjectSettings.GlobalizePath(SavePath));
		ResourceSaver.Save(this, GetPathSave());
	}
	
	public void Load()
	{
		string globalPath = ProjectSettings.GlobalizePath(GetPathSave());

		if (!System.IO.File.Exists(globalPath))
		{
			Volume = 1;
			return;
		}
		
		VolumeSetting setting = ResourceLoader.Load<VolumeSetting>(GetPathSave());
		Volume = setting.Volume;
	}
	
	private string GetPathSave()
		=> SavePath + _saveName + ".tres";
}
