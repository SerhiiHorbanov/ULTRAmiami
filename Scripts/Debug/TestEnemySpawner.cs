using Godot;
using ULTRAmiami.Controllers;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class TestEnemySpawner : Node
{
    [Export] private PackedScene _spawnedEnemyPackedScene;
    [Export] private Unit _player;
    [Export] private Timer _timer;
    [Export] private Node2D _spawnPosition;
    
    public override void _Ready()
    {
        _timer.Timeout += OnTimerTimeout;
        _player.OnDeath += OnPlayerDeath;
    }

    public override void _ExitTree()
    {
        if (_player is not null)
            _player.OnDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath(Hit hit)
    {
        _player = null;
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
