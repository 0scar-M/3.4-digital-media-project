using Godot;
using System;
using System.Collections.Generic;
using static System.Math;

public partial class RocketNode : CharacterBody2D
{
	public LevelFile File;
	public RocketFile ThisFile;
	public GameNode Game;
	
	public float Mass;
	public bool ThrustOn = false;
	public float ThrustLevel = 0f;
	public float ThrustIncrement = 0.5f;
	public float ThrustImpulse;
	
	public Sprite2D RocketSprite;
	public Sprite2D FlameSprite;
	public CollisionShape2D CollisionBody;
	public KinematicCollision2D Collision;
	
	private float _newThrustLevel;
	
	public void Init(LevelFile parsedFile, RocketFile parsedThisFile, GameNode parsedGame)
	{
		/*
		Sets instance-specific node data. 
		_Ready() doesn't work for this because the script is attached after the node enters the scene.
		*/
		
		this.File = parsedFile;
		this.ThisFile = parsedThisFile;
		this.Game = parsedGame;
		
		var initNode = GetNode<BaseBodyNode>("../" + ThisFile.InitBody);
		
		Position = initNode.Position + PolarToCartesian(ThisFile.InitPosition.X - ((float) PI/2), initNode.Position.Length() + ThisFile.InitPosition.Y);
		Rotation = ThisFile.InitRotation;
		Scale = ThisFile.Scale;
		Velocity = ThisFile.InitVelocity;
		ThrustImpulse = ThisFile.ThrustImpulse;
		Mass = ThisFile.Mass;
		MotionMode = MotionModeEnum.Floating;
		
		RocketSprite = GetNode<Sprite2D>("RocketSprite");
		RocketSprite.Texture = (Texture2D) GD.Load(ThisFile.RocketTexturePath);
		
		FlameSprite = GetNode<Sprite2D>("FlameSprite");
		FlameSprite.Texture = (Texture2D) GD.Load(ThisFile.FlameTexturePath);
		FlameSprite.Position = ThisFile.flamePosition;
		FlameSprite.Offset = ThisFile.flameOffset;
		FlameSprite.Hide();
		
		CollisionBody = GetChild<CollisionShape2D>(2);
		CollisionBody.Name = "RocketCollisionBody";
		var collisionShape = new RectangleShape2D();
		collisionShape.Size = new Vector2(RocketSprite.Texture.GetSize().X, RocketSprite.Texture.GetSize().Y);
		CollisionBody.Shape = collisionShape;
	}
	
	public Vector2 PolarToCartesian(float theta, float radius)
	{
		return new Vector2((float) (radius * Cos(theta)), (float) (radius * Sin(theta)));
	}
	
	public Vector2 GravityAcceleration(float gravityConst, Vector2 objectPosition, Vector2 bodyPosition, float objectMass, float bodyMass)
	{
		return PolarToCartesian(objectPosition.AngleToPoint(bodyPosition), gravityConst * objectMass * objectMass * bodyMass / objectPosition.DistanceTo(bodyPosition));
	}
	
	public override void _Ready()
	{
		// Stop physcis process from running so everything can initialise properly.
		SetPhysicsProcess(false);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("RocketRotateLeft"))
			RotationDegrees -= 1f;
		else if (Input.IsActionPressed("RocketRotateRight"))
			RotationDegrees += 1f;
		
		if (Input.IsActionPressed("RocketThrustUp"))
		{
			_newThrustLevel = ThrustLevel + ThrustIncrement;
			if (ThrustOn & (_newThrustLevel <= 1))
				ThrustLevel = _newThrustLevel;
		}
		
		if (Input.IsActionPressed("RocketThrustDown"))
		{
			_newThrustLevel = ThrustLevel - ThrustIncrement;
			if (ThrustOn & (_newThrustLevel >= 0))
				ThrustLevel = _newThrustLevel;
		}
		
		if (Input.IsActionJustPressed("RocketThrustToggle"))
		{
			ThrustOn = !ThrustOn;
			if (ThrustOn)
				FlameSprite.Show();
			else
				FlameSprite.Hide();
		}
		
		if (ThrustOn)
		{
			// Update Velocity based on Thrust
			Velocity += PolarToCartesian((float) (Rotation - PI/2), ThrustImpulse * ThrustLevel);
			FlameSprite.Scale = new Vector2(ThrustLevel, ThrustLevel);
		}
		
		//Update Velocity based on gravity
		// CentreBody
		Velocity += GravityAcceleration(Game.GravityConst, Position, Game.CentreBody.Position, Mass, Game.CentreBody.Mass);
		
		// Bodies
		foreach (var b in Game.Bodies.Values)
		{
			Velocity += GravityAcceleration(Game.GravityConst, Position, b.Position, Mass, b.Mass);
		}
		
		// Update position
		Collision = MoveAndCollide(Velocity * (float) delta);
		
		if (Collision != null)
		{
			// Collide
			var reflect = Collision.GetRemainder().Bounce(Collision.GetNormal());
			Velocity = Velocity.Bounce(Collision.GetNormal());
			MoveAndCollide(reflect);
		}
	}
}
