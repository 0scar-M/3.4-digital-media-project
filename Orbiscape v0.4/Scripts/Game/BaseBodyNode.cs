using Godot;
using System;

public abstract partial class BaseBodyNode : CharacterBody2D
{
	public LevelFile File;
	public BaseBodyFile ThisFile;
	public GameNode Game;
	public float Mass;
	
	public override void _Ready()
	{
		// Stop physcis process from running so everything can initialise properly.
		SetPhysicsProcess(false);
	}
}
