using System.Collections.Generic;
using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.UI;

public partial class Crosshair : Node2D
{
	private record struct Corner(CrosshairCornerInfo Info, Sprite2D Sprite);

	private Corner[] _corners;
	private List<Corner> _spreadShowingCorners;
	private Sprite2D _center;
	
	private CrosshairInfo _info;
	[Export] private CrosshairInfo _defaultInfo;

	[Export] private Unit _player;
	
	public override void _Process(double delta)
	{
		if (_spreadShowingCorners is null)
			return;
		
		float dist = GetCurrentSpreadDistance();
		
		foreach (Corner corner in _spreadShowingCorners)
		{
			float currDist = float.Max(dist, corner.Info.MinDistance);
			corner.Sprite.Position = corner.Info.Direction * currDist;
		}
	}
	
	private void ApplyInfo(CrosshairInfo info)
	{
		if (info is null)
			return;
		
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
		_spreadShowingCorners = [];
		
		for (int i = 0; i < _corners.Length; i++)
		{
			CrosshairCornerInfo cornerInfo = _info.Corners[i];
			Sprite2D corner = GenerateCorner(cornerInfo);

			_corners[i] = new(cornerInfo, corner);
		}
	}

	private Sprite2D GenerateCorner(CrosshairCornerInfo info)
	{
		Sprite2D corner = new();
		
		corner.Texture = info.Texture;
		corner.SelfModulate = info.Color;
		corner.Rotation = info.RotationRad;
		corner.Position = info.Direction;
		corner.Scale = info.Scale;
		AddChild(corner);

		if (info.ShowsSpread)
		{
			_spreadShowingCorners.Add(new(info, corner));
			corner.Position *= GetCurrentSpreadDistance();
		}
		
		return corner;
	}

	private float GetCurrentSpreadDistance()
	{
		float distanceFromPlayer = Position.Length();
		float alpha = _info.HalfShownSpreadRad;
		
		return float.Sin(alpha) * distanceFromPlayer;
	}

	private void UpdateCrosshair(Weapon weapon)
		=> ApplyInfo(weapon?.CrosshairInfo ?? _defaultInfo);
}
