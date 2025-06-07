using System.Collections.Generic;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons;

public partial class MeleeAttacker : Node2D
{
	[Export] private Area2D _attackArea;
	[Export] private Timer _attackDurationTimer;
	
	[Export] private float _maxStamina;
	private float _stamina;
	[Export] private float _attackStaminaCost;
	
	[Export] private Unit _ignoredUnit;
	[Export] private float _damage;

	[Export] private AudioStreamPlayer2D _attackAudio;
	[Export] private AudioStreamPlayer2D _hitAudio;
	
	private readonly Dictionary<Node2D, IAttackable> _attackableInArea = [];

	[Signal]
	public delegate void OnAttackingEventHandler();
	[Signal]
	public delegate void OnHittingEventHandler(Vector2 globalPosition);
	
	private bool IsAttacking
		=> !_attackDurationTimer.IsStopped();
	
	public override void _Ready()
	{
		_stamina = _maxStamina;
	}

	public override void _Process(double delta)
	{
		_stamina += (float)delta;
		_stamina = float.Min(_stamina, _maxStamina);
	}

	public void TryAttack()
	{
		if (_stamina < _attackStaminaCost)
			return;
		Attack();
	}
	
	private void Attack()
	{
		_attackAudio.Play();
		_attackDurationTimer.Start();
		_stamina -= _attackStaminaCost;
		
		foreach ((Node2D node, IAttackable attackable) in _attackableInArea)
		{
			Hit(attackable, node.GlobalPosition);
		}
		
		EmitSignalOnAttacking();
	}
	
	private void Hit(AttackableAsGodotObject attackable, Vector2 position)
		=> Hit(attackable.Attackable, position);
	
	private void Hit(IAttackable attackable, Vector2 position)
	{
		_attackAudio.Stop();
		
		if (attackable.Bleeds)
		{
			_hitAudio.Play();
			EmitSignalOnHitting(GlobalPosition);
		}
		
		Hit hit = new(Vector2.FromAngle(Rotation), _damage);
		attackable.Hit(hit);
	}

	private void OnBodyEntered(Node2D body)
	{
		IAttackable attackable = body.GetAncestor<IAttackable>();

		if (attackable is null)
			return;

		if (attackable as Unit == _ignoredUnit)
			return;

		if (!IsAttacking)
		{
			_attackableInArea.Add(body, attackable);
			return;
		}
		
		AttackableAsGodotObject godotObject = new(attackable);
		CallDeferred(MethodName.Hit, godotObject, body.GlobalPosition);
	}
	
	private void OnBodyExited(Node2D body)
		=> _attackableInArea.Remove(body);
}