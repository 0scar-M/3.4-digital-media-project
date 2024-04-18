using Godot;
using System;

public partial class CentreBodyNode : BaseBodyNode
{
	//public CentreBodyFile ThisFile;
	
	public void Init(LevelFile parsedFile, CentreBodyFile parsedThisFile, GameNode parsedGame)
	{
		/*
		Sets instance-specific node data. 
		_Ready() doesn't work for this because properties might depend on other nodes that haven't been created yet.
		*/
		
		this.File = parsedFile;
		this.ThisFile = parsedThisFile;
		this.Game = parsedGame;
		
		Name = ThisFile.Name;
		Position = ThisFile.InitPosition;
		Mass = ThisFile.Mass;
		MotionMode = MotionModeEnum.Floating;
		
		AddChild(new Sprite2D());
		var sprite = GetChild<Sprite2D>(0);
		sprite.Name = $"{this.Name}Sprite";
		sprite.Texture = (Texture2D) GD.Load(ThisFile.TexturePath);
		sprite.Scale = new Vector2(ThisFile.Radius / (ThisFile.TextureRadius * sprite.Texture.GetWidth() / 2), ThisFile.Radius / (ThisFile.TextureRadius * sprite.Texture.GetHeight() / 2));
		
		AddChild(new CollisionShape2D());
		var collisionBody = GetChild<CollisionShape2D>(1);
		collisionBody.Name = $"{this.Name}CollisionShape";
		var collisionShape = new CircleShape2D();
		collisionShape.Radius = ThisFile.Radius;
		collisionBody.Shape = collisionShape;
	}
}
