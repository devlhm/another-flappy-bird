using System;
using Godot;

namespace AnotherFlappyBird.Scripts;

public partial class Player : CharacterBody2D
{
	[Export] private float _gravityMult =  .8f;
	[Export] private float _jumpVelocity = -190.0f;
	[Export] private Area2D _hurtbox;
	[Export] private AnimationPlayer _animPlayer;
	[Export] private Vector2 _spawnPos;
	[Signal] public delegate void DeathEventHandler();

	private bool _started = false;
	private bool _dead = false;

	public override void _Ready()
	{
		_hurtbox.BodyEntered += OnDeath;
		_hurtbox.AreaEntered += OnDeath;
		_animPlayer.Play("fly");
		
	}

	private void OnDeath(Node2D body)
	{
		SetPhysicsProcess(false);
		EmitSignal(SignalName.Death);
		_animPlayer.Stop();
		_started = false;
		_dead = true;
	}

	public void OnStart()
	{
		GD.Print("started");
		_started = true;
		_animPlayer.Play("fly");
	}

	public void OnInit()
	{
		RotationDegrees = 0;
		Position = _spawnPos;
		_animPlayer.Play("fly");
		_dead = false;
		SetPhysicsProcess(true);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (_started) velocity += GetGravity() * (float)delta * _gravityMult;
		else velocity = Vector2.Zero;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && !_dead)
		{
			velocity.Y = _jumpVelocity;
		}

		Velocity = velocity;
		
		RotationDegrees = Mathf.Lerp(RotationDegrees, Mathf.Clamp(Velocity.Y, -15, 15), 0.15f);
		MoveAndSlide();
	}
}