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
        _player.OnDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        _player = null;
    }

    private void OnTimerTimeout()
    {
        SpawnEnemy();
    }
    
    private void SpawnEnemy()
    {
        Node instantiatedScene = _spawnedEnemyPackedScene.Instantiate();
        PistolEnemyUnitController controller = instantiatedScene.GetChild<PistolEnemyUnitController>();
        Unit unit = instantiatedScene.GetChild<Unit>();
        
        controller.TargetUnit = _player;
        unit.Position = _spawnPosition.Position;
        
        GetParent().AddChild(instantiatedScene);
    }
}