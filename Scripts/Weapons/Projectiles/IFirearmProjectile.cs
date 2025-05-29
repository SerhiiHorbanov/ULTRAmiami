using System.ComponentModel;
using Godot;

namespace ULTRAmiami.Weapons.Projectiles;

public interface IFirearmProjectile
{
	public void Initialize(Vector2 globalPosition, Vector2 shootingAt);
}
