using Godot;
using System;

public partial class BodyNode : BaseBodyNode
{
	public void Init(LevelFile parsedFile, BodyFile parsedThisFile, GameNode parsedGame)
	{
		/*
		Sets instance-specific node data. 
		_Ready() doesn't work for this because properties might depend on other nodes that haven't been created yet.
		*/
		
		this.File = parsedFile;
		this.ThisFile = parsedThisFile;
		this.Game = parsedGame;
		
		Name = ThisFile.Name;
		Scale = new Vector2(ThisFile.Radius, ThisFile.Radius);
		Mass = ThisFile.Mass;
		MotionMode = MotionModeEnum.Floating;
		
		AddChild(new Sprite2D());
		var bodySprite = GetChild<Sprite2D>(0);
		bodySprite.Name = $"{this.Name}Sprite";
		bodySprite.Texture = (Texture2D) GD.Load(ThisFile.TexturePath);
		
		AddChild(new CollisionShape2D());
		var bodyCollision = GetChild<CollisionShape2D>(1);
		bodyCollision.Name = $"{this.Name}CollisionShape";
		bodyCollision.Shape = new CircleShape2D();
	}
}
