using Godot;
using System;
using Godot.Collections;

public partial class ParticleRestarter : Node
{
	[Export] private Array<CpuParticles2D> _particles;
	
	public override void _Ready()
	{
		foreach (CpuParticles2D particle in _particles)
			particle.Restart();
	}
}
