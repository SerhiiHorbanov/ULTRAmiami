using Godot;

namespace ULTRAmiami.Weapons.Audio;

public partial class ShotgunPumper : AudioStreamPlayer2D
{
	[Export] private Weapon _shotgun;
	[Export] private Timer _pumpingTimer;
	
	public override void _Ready()
	{
		_shotgun.OnShoot += TryPumping;
		_shotgun.OnReloadFinished += () => TryPumping(1);
		_shotgun.OnUnitChanged += Stop;
		
		_pumpingTimer.Timeout += () => Play();
	}

	public override void _ExitTree()
	{
		//Stop();
	}

	private void TryPumping(int ammo)
	{
		if (ammo == 0)
			return;
		
		_pumpingTimer.Start();
	}
}
