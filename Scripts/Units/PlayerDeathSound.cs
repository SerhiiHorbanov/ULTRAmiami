using Godot;
using System;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Units;

public partial class PlayerDeathSound : AudioStreamPlayer
{
	public void Start(Vector2 _)
		=> Start();
	public void Start()
	{
		this.MakeSiblingOf(GetParent());
		Play();
	}
}
