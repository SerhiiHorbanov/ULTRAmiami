using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Debug;

public partial class TestEnemySpawner : Node
{
    [Export] private PackedScene _spawnedEnemyPackedScene;
    [Export] private Timer _timer;
    [Export] private Node2D _spawnPosition;
    
    public override void _Ready()
    {
        _timer.Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        SpawnEnemy();
    }
    
    private void SpawnEnemy()
    {
        Unit unit = _spawnedEnemyPackedScene.Instantiate<Unit>();
        unit.Position = _spawnPosition.Position;
        
        GetParent().AddChild(unit);
    }
}
