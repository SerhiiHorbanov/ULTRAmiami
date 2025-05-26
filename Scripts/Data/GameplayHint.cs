using Godot;
using ULTRAmiami.UI;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class GameplayHint : Resource
{
	[Export] public string Text { get; private set; }
	[Export] public float Time;

	[Export] public bool OverrideCurrentHint { get; private set; }

	public void Display()
	{
		if (OverrideCurrentHint)
			GameplayHintText.OverrideCurrentHint(this);
		else
			GameplayHintText.AddHintToQueue(this);
	}
}
