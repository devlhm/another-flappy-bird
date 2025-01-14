using System;
using Godot;

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
	[Export] private RichTextLabel _scoreLabel;
	[Export] private RichTextLabel _highScoreLabel;
	
	private bool _dead;
	private bool _started;
	
	private int _score;
	private int _highScore;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pipeSpawnTimer.Timeout += SpawnPipe;
		_player.Death += OnDeath;
		_groundAnimPlayer.Play("scroll");
		SetHighScore(SaveManager.LoadScore());
	}

	private void SetScore(int score)
	{
		_score = score;
		_scoreLabel.Text = $"[center]{score.ToString()}[/center]";
		
		if (score > _highScore)
			SetHighScore(score);
	}

	private void SetHighScore(int score)
	{
		_highScore = score;
		_highScoreLabel.Text = $"[center]HIGH: {_highScore.ToString()}[/center]";
	}

	private void StartGame()
	{
		GD.Randomize();
		_pipeSpawnTimer.Start();
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
		AddChild(pipe);
	}

	private void OnScore()
	{
		SetScore(_score + 1);
	}

	private void OnDeath()
	{
		_started = false;
		_dead = true;
		_groundAnimPlayer.Pause();
		_pipeSpawnTimer.Stop();
		GetTree().CallGroup("pipe", "OnDeath");
		SaveManager.SaveScore(_highScore);
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
		SetScore(0);
	}
}
