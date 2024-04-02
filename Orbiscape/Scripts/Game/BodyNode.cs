using Godot;
using System;

public partial class BodyNode : CharacterBody2D
{
	public BodyFile File;
	
	public void Init()
	{
		//body.MotionMode.MotionModeFloating = 1;
		Name = File.Name;
		Scale = new Vector2(File.Radius, File.Radius);
		
		AddChild(new Sprite2D());
		var bodySprite = GetChild<Sprite2D>(0);
		bodySprite.Name = $"{this.Name}Sprite";
		bodySprite.Texture = (Texture2D) GD.Load(File.TexturePath);
		
		AddChild(new CollisionShape2D());
		var bodyCollision = GetChild<CollisionShape2D>(1);
		bodyCollision.Name = $"{this.Name}CollisionShape";
		bodyCollision.Shape = new CircleShape2D();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
	}
}
