using Godot;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Effects;

public partial class PlayerArmHitParticlesSpawner : Node2D
{
	[Export] private Node _treeSiblingOfParticles;
	[Export] private PackedScene _particlesScene;
	
	private void SpawnParticles(Vector2 position)
	{
		MyParticles particles = _particlesScene.Instantiate<MyParticles>();
		particles.MakeSiblingOf(_treeSiblingOfParticles);
		particles.GlobalPosition = position;
		particles.GlobalRotation = GlobalRotation;
		particles.Emit();
	}
}
