using Godot;

namespace AnotherFlappyBird.Scripts;

public partial class Pipe : AnimatableBody2D
{
	[Signal] 
	public void delegate DespawnEventHandler();
	
	[Export] private float _speed = 10f;
	[Export] private Vector2 _pipeDespawnPos;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		var newPos = new Vector2(Position.X - (_speed * (float) delta), Position.Y);
		Position = newPos;
	}
}