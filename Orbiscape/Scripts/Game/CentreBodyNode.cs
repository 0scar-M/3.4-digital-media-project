using Godot;
using System;

public partial class CentreBodyNode : CharacterBody2D
{
	public LevelFile File;
	
	public void Init()
	{
		//CentreBody.MotionMode.MotionModeFloating = 1;
		Name = File.CentreBody.Name;
		Position = File.CentreBody.InitPosition;
		Scale = new Vector2(File.CentreBody.Radius, File.CentreBody.Radius);
		
		AddChild(new Sprite2D());
		var centreBodySprite = GetChild<Sprite2D>(0);
		centreBodySprite.Name = $"{this.Name}Sprite";
		centreBodySprite.Texture = (Texture2D) GD.Load(File.CentreBody.TexturePath);
		
		AddChild(new CollisionShape2D());
		var centreBodyCollision = GetChild<CollisionShape2D>(1);
		centreBodyCollision.Name = $"{this.Name}CollisionShape";
		centreBodyCollision.Shape = new CircleShape2D();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
	}
}
