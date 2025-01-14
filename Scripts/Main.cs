using Godot;
using System;

namespace AnotherFlappyBird.Scripts;
public partial class Main : Node2D
{
	
	[Export] private PackedScene _pipeScene;
	[Export] private Marker2D _pipeSpawnPos;
	[Export] private Marker2D _pipeDespawnPos;
	[Export] private AnimationPlayer _groundAnimPlayer;
	[Export] private float _minPipeY;
	[Export] private float _maxPipeY;
	[Export] private Player _player;
	[Export] private Timer _pipeSpawnTimer;
	[Export] private Node _pausable;
	[Export] private Label _scoreLabel;
	private bool _dead = false;

	private bool _started = false;
	private int _score = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pipeSpawnTimer.Timeout += SpawnPipe;
		_player.Death += OnDeath;
		_groundAnimPlayer.Play("scroll");
	}

	private void SetScore(int score)
	{
		_score = score;
		_scoreLabel.Text = score.ToString();
	}

	private void StartGame()
	{
		GD.Print("start game");
		_pipeSpawnTimer.Start();
		GD.Randomize();
		GetTree().Paused = false;
		SpawnPipe();
		_started = true;
		_groundAnimPlayer.Play("scroll");
		_player.OnStart();
	}

	private void SpawnPipe()
	{
		var pipe = (Pipe)_pipeScene.Instantiate();
		pipe.Score += OnScore; 
		pipe.Position = new Vector2(_pipeSpawnPos.Position.X, (float) Math.Round(GD.RandRange(_minPipeY, _maxPipeY)));
		pipe.DespawnPos = _pipeDespawnPos;
		_pausable.AddChild(pipe);
	}

	private void OnScore()
	{
		SetScore(_score + 1);
	}

	private void OnDeath()
	{
		_started = false;
		_dead = true;
		GetTree().Paused = true;
		_groundAnimPlayer.Pause();
		GD.Print("Death");
		_pipeSpawnTimer.Stop();
		SetScore(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept") && !_started)
		{
			if (_dead) Init();
			else StartGame();
		}
	}

	private void Init()
	{
		GetTree().CallGroup("pipe", "Destroy");
		_dead = false;
		_player.OnInit();
		_groundAnimPlayer.Play("scroll");
	}
}
