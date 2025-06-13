using Godot;
using Godot.Collections;

namespace ULTRAmiami.Units;

public partial class BloodynessStagesTexturesApplier : Node
{
	[Export] private PlayerBloodyness _bloodyness;
	[Export] private Array<float> _bloodynessStagesThresholds;
	[Export] private Sprite2D _sprite;

	private int _currentBloodynessStage;
	
	private float Bloodyness
		=> _bloodyness.Bloodyness;

	public override void _Process(double delta)
	{ 
		int newBloodynessStage = CalculateCurrentBloodynessStage();
		if (newBloodynessStage != _currentBloodynessStage)
		{
			_sprite.Frame = newBloodynessStage;
			_currentBloodynessStage = newBloodynessStage;
		}
	}
	
	private int CalculateCurrentBloodynessStage()
	{
		bool isOnLastBloodynessStage = _currentBloodynessStage == _bloodynessStagesThresholds.Count - 1;
		bool isTooBloodyForCurrentStage = !isOnLastBloodynessStage && Bloodyness > _bloodynessStagesThresholds[_currentBloodynessStage];
		
		if (isTooBloodyForCurrentStage)
			return _currentBloodynessStage + 1;
		
		bool isOnFirstStage = _currentBloodynessStage == 0;
		bool isNotBloodyEnoughForCurrentStage = !isOnFirstStage && Bloodyness < _bloodynessStagesThresholds[_currentBloodynessStage - 1];

		if (isNotBloodyEnoughForCurrentStage)
			return _currentBloodynessStage - 1;
		
		return _currentBloodynessStage;
	}
}
