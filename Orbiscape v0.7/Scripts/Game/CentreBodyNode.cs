using Godot;
using System;

public partial class CentreBodyNode : BaseBodyNode
{
	public CentreBodyFile ThisFile;
	
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
		MotionMode = MotionModeEnum.Floating;
		Mass = ThisFile.Mass;
		Radius = ThisFile.Radius;
		Position = ThisFile.InitPosition;
		
		Sprite = GetNode<Sprite2D>("Sprite");
		Sprite.Texture = (Texture2D) GD.Load(ThisFile.TexturePath);
		Sprite.Scale = new Vector2(ThisFile.Radius / (ThisFile.TextureRadius * Sprite.Texture.GetWidth() / 2), ThisFile.Radius / (ThisFile.TextureRadius * Sprite.Texture.GetHeight() / 2));
		
		CollisionBody = GetNode<CollisionShape2D>("CollisionBody");
		var collisionShape = new CircleShape2D();
		collisionShape.Radius = ThisFile.Radius;
		CollisionBody.Shape = collisionShape;
	}
}
