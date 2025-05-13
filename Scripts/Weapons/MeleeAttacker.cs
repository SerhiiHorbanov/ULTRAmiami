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
	
	private readonly Dictionary<Node2D, Unit> _unitsInArea = [];

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
		
		foreach (Unit unit in _unitsInArea.Values)
		{
			HitUnit(unit);
			EmitSignalOnHitting(unit.GlobalPosition);
		}
		
		EmitSignalOnAttacking();
	}
	
	private void HitUnit(Unit unit)
	{
		Hit hit = new(Vector2.FromAngle(Rotation), _damage);
		unit.Hit(hit);
	}

	private void OnBodyEntered(Node2D body)
	{
		Unit unit = body.GetAncestor<Unit>();

		if (unit is null)
			return;

		if (_ignoredUnit == unit)
			return;

		if (IsAttacking)
		{
			CallDeferred(MethodName.HitUnit, unit);
			return;
		}
		
		_unitsInArea.Add(body, unit);
	}
	
	private void OnBodyExited(Node2D body)
		=> _unitsInArea.Remove(body);
}