using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;

namespace ULTRAmiami.UI;

public partial class Crosshair : Node2D
{
	private record struct Corner(CrosshairCornerInfo Info, Sprite2D Sprite);

	private Corner[] _corners;
	private Sprite2D _center;
	
	[Export] private CrosshairInfo _info;
	[Export] private float _defaultCornersDistance;

	[Export] private Unit _player;

	public override void _Ready()
	{
		ApplyInfo(_info);
	}

	public override void _Process(double delta)
	{
		
	}

	private void ApplyInfo(CrosshairInfo info)
	{
		_info = info;
		GenerateCorners();
		
		_center?.QueueFree();
		
		_center = new();
		_center.Scale = _info.CenterScale;
		_center.Texture = _info.CenterTexture;
		AddChild(_center);
	}
	
	private void GenerateCorners()
	{
		if (_corners is not null)
			foreach (Corner cornerInfo in _corners)
				cornerInfo.Sprite.QueueFree();
		
		_corners = new Corner[_info.Corners.Count];
		
		for (int i = 0; i < _corners.Length; i++)
		{
			CrosshairCornerInfo cornerInfo = _info.Corners[i];
			Sprite2D corner = GenerateCorner(cornerInfo);

			_corners[i] = new(cornerInfo, corner);
		}
	}

	private Sprite2D GenerateCorner(CrosshairCornerInfo cornerInfo)
	{
		Sprite2D corner = new();
		
		corner.Texture = cornerInfo.Texture;
		corner.SelfModulate = cornerInfo.Color;
		corner.Rotation = cornerInfo.RotationRad;
		corner.Position = cornerInfo.Direction * GetCurrentCornersDistance();
		corner.Scale = cornerInfo.Scale;
		AddChild(corner);

		return corner;
	}

	private float GetCurrentCornersDistance()
	{
		return _defaultCornersDistance;
	}
}
