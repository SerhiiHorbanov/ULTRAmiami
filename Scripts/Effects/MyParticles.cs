using Godot;
using Godot.Collections;

namespace ULTRAmiami.Effects;

public partial class MyParticles : Node2D
{
	[Export] private Array<CpuParticles2D> _particlesToStartEmitting;
	[Export] private Array<CpuParticles2D> _particlesToWaitFor;

	public void Emit()
	{
		if (_particlesToStartEmitting is null)
			return;
		
		foreach (CpuParticles2D particle in _particlesToStartEmitting)
			particle.Restart();
	}

	public override void _Process(double delta)
	{
		if (_particlesToWaitFor is null)
		{
			QueueFree();
			return;
		}
		
		foreach (CpuParticles2D particle in _particlesToWaitFor)
		{
			if (particle.IsEmitting())
				return;
		}
		
		QueueFree();
	}
}