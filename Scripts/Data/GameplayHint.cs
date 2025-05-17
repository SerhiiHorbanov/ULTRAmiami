using Godot;
using ULTRAmiami.UI;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class GameplayHint : Resource
{
	[Export] public string Text { get; private set; }
	[Export] public float Time { get; private set; }

	public void Display()
	{
		GameplayHintText.AddHintToQueue(this);
	}
}
