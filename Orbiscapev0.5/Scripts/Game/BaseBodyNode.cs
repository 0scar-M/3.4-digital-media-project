using Godot;
using System;

public abstract partial class BaseBodyNode : CharacterBody2D
{
	public LevelFile File;
	public GameNode Game;
	
	public Sprite2D Sprite;
	public CollisionShape2D CollisionBody;
	
	public float Radius;
	public float Mass;
	
	public override void _Ready()
	{
		// Stop physcis process from running so everything can initialise properly.
		SetPhysicsProcess(false);
	}
}
