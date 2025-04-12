using Godot;

namespace ULTRAmiami.Weapons.Visuals;

public partial class DeleteParticleOnFinished : CpuParticles2D
{
	public override void _Ready()
		=> Finished += QueueFree;
}