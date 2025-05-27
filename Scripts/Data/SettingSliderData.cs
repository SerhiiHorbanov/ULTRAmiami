using System.IO;
using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class SettingSliderData : Resource
{
	[Export] public float Value;
	[Export] private float _defaultValue = 1;
	
	public void Save(string dir, string fileName)
	{
		Directory.CreateDirectory(ProjectSettings.GlobalizePath(dir)); 
		ResourceSaver.Save(this, dir + fileName);
	}

	public void Load(string path)
	{
		string globalPath = ProjectSettings.GlobalizePath(path);

		if (!File.Exists(globalPath))
		{
			Value = _defaultValue;
			return;
		}
		
		SettingSliderData setting = ResourceLoader.Load<SettingSliderData>(path);
		Value = setting.Value;
	}
}
