using System;
using Godot;

namespace AnotherFlappyBird.Scripts;

public partial class Pipe : Area2D
{

	
	[Export] private float _speed = 10f;

	[Export]
	private Area2D _scoreArea;
	[Signal] public delegate void ScoreEventHandler();
	public Marker2D DespawnPos { get; set; }
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("pipe");
		_scoreArea.BodyEntered += OnScoreAreaEntered;
	}

	private void OnScoreAreaEntered(Node2D body)
	{
		if (body is Player) EmitSignal(SignalName.Score);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		var newPos = new Vector2(Position.X - (_speed * (float) delta), Position.Y);
		Position = newPos;
		if(Position.X <= DespawnPos.Position.X) Destroy();
	}

	private void Destroy()
	{
		GetParent().RemoveChild(this);
		QueueFree();
	}

	public void OnDeath()
	{
		SetPhysicsProcess(false);
	}
}
