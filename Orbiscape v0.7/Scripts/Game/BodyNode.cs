using Godot;
using System;
using static System.Math;

// This is mostly redundant as there are no levels with multiple bodies, but the capability is there if I ever do it in the future.
public partial class BodyNode : BaseBodyNode
{
	public BodyFile ThisFile;
	
	public Sprite2D SOISprite;
	
	public float OrbitRadius;
	public float OrbitAngle;
	public float OrbitAngleSpeed;
	public float InfluenceRadius;
	
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
		MotionMode = MotionModeEnum.Floating;
		Mass = ThisFile.Mass;
		Radius = ThisFile.Radius;
		OrbitRadius = ThisFile.OrbitRadius;
		OrbitAngle = ThisFile.InitOrbitAngle*(float)Math.PI/180;
		//OrbitAngleSpeed = (float)(Math.Sqrt(Game.GravityConst * Game.CentreBody.Mass / OrbitRadius) / 2*Math.PI*OrbitRadius);
		OrbitAngleSpeed = (float)Math.Sqrt(Game.GravityConst * Game.CentreBody.Mass / OrbitRadius) / OrbitRadius;
		if (!ThisFile.IsOrbitClockwise)
			OrbitAngleSpeed *= -1;
		Position = Game.CentreBody.Position + _polarToCartesian(OrbitAngle - (float) Math.PI/2, OrbitRadius);
		InfluenceRadius = OrbitRadius * (float) Math.Pow((double) (Mass / Game.CentreBody.Mass), 2.0/5.0);
		
		Sprite = GetNode<Sprite2D>("Sprite");
		Sprite.Texture = (Texture2D) GD.Load(ThisFile.TexturePath);
		Sprite.Scale = new Vector2(ThisFile.Radius / (ThisFile.TextureRadius * Sprite.Texture.GetWidth() / 2), ThisFile.Radius / (ThisFile.TextureRadius * Sprite.Texture.GetHeight() / 2));
		
		SOISprite = GetNode<Sprite2D>("SOISprite");
		SOISprite.Scale = new Vector2((InfluenceRadius / (Sprite.Texture.GetWidth() / 2)), (InfluenceRadius / (Sprite.Texture.GetWidth() / 2)));
		
		CollisionBody = GetNode<CollisionShape2D>("CollisionBody");
		var collisionShape = new CircleShape2D();
		collisionShape.Radius = ThisFile.Radius;
		CollisionBody.Shape = collisionShape;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		OrbitAngle += OrbitAngleSpeed * (float) delta;
		Position = Game.CentreBody.Position + _polarToCartesian(OrbitAngle - (float) Math.PI/2, OrbitRadius);
	}
	
	private Vector2 _polarToCartesian(float theta, float radius)
	{
		return new Vector2((float) (radius * Cos(theta)), (float) (radius * Sin(theta)));
	}
}
