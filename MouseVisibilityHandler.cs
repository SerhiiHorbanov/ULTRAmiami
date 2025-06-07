using Godot;

namespace ULTRAmiami;

public partial class MouseVisibilityHandler : Node
{
	private static MouseVisibilityHandler _instance;

	private int _visibilityCounter;

	public static void ChangeVisibility(bool positive)
	{
		if (positive)
			IncrementVisibility();
		else
			DecrementVisibility();
	}
	
	public static void DecrementVisibility()
	{
		if (_instance is null)
			return;
		
		_instance._visibilityCounter--;
		_instance.UpdateVisibility();
	}

	public static void IncrementVisibility()
	{
		if (_instance is null)
			return;
		
		_instance._visibilityCounter++;
		_instance.UpdateVisibility();
	}
	
	public override void _Ready()
	{
		_instance ??= this;
	}

	public override void _ExitTree()
	{
		if (_instance == this)
			_instance = null;
		
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	private void UpdateVisibility()
		=> Input.MouseMode = _visibilityCounter == 0 ? Input.MouseModeEnum.Hidden : Input.MouseModeEnum.Visible;
}
