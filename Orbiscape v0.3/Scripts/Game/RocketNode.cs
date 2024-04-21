using Godot;
using System;

public partial class RocketNode : CharacterBody2D
{
	public LevelFile File;
	public bool EngineOn = false;
	public float ThrustLevel = 0f;
	public float ThrustRate;
	
	public void Init()
	{
		/*
		Sets instance-specific node data. 
		_Ready() doesn't work for this because the script is attached after the node enters the scene.
		*/
		
		Name = "Rocket";
		//this.MotionModeEnum.Floating = 1;
		Position = File.Rocket.InitPosition;
		Rotation = File.Rocket.InitRotation;
		Scale = new Vector2(File.Rocket.Scale, File.Rocket.Scale);
		Velocity = File.Rocket.InitVelocity;
		ThrustRate = File.Rocket.ThrustRate;
		
		AddChild(new Sprite2D());
		var rocketSprite = GetChild<Sprite2D>(0);
		rocketSprite.Name = "RocketSprite";
		rocketSprite.Texture = (Texture2D) GD.Load(File.Rocket.RocketTexturePath);
		
		AddChild(new Sprite2D());
		var rocketFlameSprite = GetChild<Sprite2D>(1);
		rocketFlameSprite.Name = "RocketFlameSprite";
		rocketFlameSprite.Texture = (Texture2D) GD.Load(File.Rocket.FlameTexturePath);
		rocketFlameSprite.Position = new Vector2(-25, 2000);
		rocketFlameSprite.Hide();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
	}
}
