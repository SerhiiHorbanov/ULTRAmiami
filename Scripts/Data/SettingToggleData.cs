using System.IO;
using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class SettingToggleData : Resource
{
	[Export] public bool Value;
	[Export] public bool _defaultValue;
	
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
		
		SettingToggleData setting = ResourceLoader.Load<SettingToggleData>(path);
		Value = setting.Value;
	}
}
