using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami;

public partial class Level : Node
{
	[Export] private LevelInfo _info;

	private void OnCompleted()
	{
		if (_info is null)
			return;

		if (!_info.HasNextLevel)
			return;
			
		_info.SaveCompletion(PlayerScore.Current);
		_info.LoadNextLevel(this);
	}
}
