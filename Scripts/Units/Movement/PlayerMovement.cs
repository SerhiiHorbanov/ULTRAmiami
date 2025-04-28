using Godot;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Units.Movement;

public partial class PlayerMovement : UnitMovement
{
	[ExportGroup("Redirection")]
	[Export] private float _maxRedirectionSpeed;
	[Export] private float _redirectionAcceleration;
	private bool _isRedirectioning;
	private bool _isRecoveringFromRedirection;

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		UpdateIsRedirectioning((float)delta);
	}

	private void UpdateIsRedirectioning(float deltaSeconds)
	{
		Vector2 targetVelocity = TargetDirection * GetMaxSpeed();
		Vector2 targetDelta = targetVelocity - Velocity;
		
		if (_isRecoveringFromRedirection)
			_isRecoveringFromRedirection = targetDelta.LengthSquared() > 50f;
		
		if (_isRedirectioning && !_isRecoveringFromRedirection)
		{
			_isRedirectioning = targetDelta.LengthSquared() > 0.1f;
			if (!_isRedirectioning)
				_isRecoveringFromRedirection = true;
		}
		else if (!IsBraking && !_isRecoveringFromRedirection)
		{
			_isRedirectioning = targetDelta.CosineSimilarity(Velocity) < -0.7f;
		}
	}

	protected override float GetMaxSpeed()
		=> _isRedirectioning ? _maxRedirectionSpeed : base.GetMaxSpeed();

	protected override float GetMaxAcceleration()
	{
		if (_isRedirectioning)
			return _redirectionAcceleration;
		return base.GetMaxAcceleration();
	}
}
