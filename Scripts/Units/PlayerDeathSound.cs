using Godot;
using System;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Units;

public partial class PlayerDeathSound : AudioStreamPlayer
{
	public void Start()
	{
		this.MakeSiblingOf(GetParent());
		Play();
	}
}
