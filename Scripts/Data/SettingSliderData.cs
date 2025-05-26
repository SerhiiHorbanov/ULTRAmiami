using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class SettingSliderData : Resource
{
	[Export] public float Value;
	[Export] private float _defaultValue = 1;
	
	public void Save(string dir, string fileName)
	{
		System.IO.Directory.CreateDirectory(ProjectSettings.GlobalizePath(dir));
		var huh = ResourceSaver.Save(this, dir + fileName);
		return;
	}

	public void Load(string path)
	{
		string globalPath = ProjectSettings.GlobalizePath(path);

		if (!System.IO.File.Exists(globalPath))
		{
			Value = _defaultValue;
			return;
		}
		
		SettingSliderData setting = ResourceLoader.Load<SettingSliderData>(path);
		Value = setting.Value;
	}
}
