#nullable enable

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
	public float ThrustIncrement = 0.01f;
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
		_Ready() doesn't work for this because properties might depend on other nodes that haven't been created yet.
		*/
		
		this.File = parsedFile;
		this.ThisFile = parsedThisFile;
		this.Game = parsedGame;
		
		var initNode = GetNode<BaseBodyNode>("../" + File.Rocket.InitBody);
		
		Position = initNode.Position + PolarToCartesian(File.Rocket.InitPosition.X - ((float) PI/2), initNode.Position.Length() + File.Rocket.InitPosition.Y);
		Rotation = File.Rocket.InitRotation;
		Scale = File.Rocket.Scale;
		Velocity = File.Rocket.InitVelocity;
		MotionMode = MotionModeEnum.Floating;
		ThrustImpulse = File.Rocket.ThrustImpulse;
		Mass = File.Rocket.Mass;
		
		RocketSprite = GetNode<Sprite2D>("RocketSprite");
		RocketSprite.Texture = (Texture2D) GD.Load(File.Rocket.RocketTexturePath);
		
		FlameSprite = GetNode<Sprite2D>("FlameSprite");
		FlameSprite.Texture = (Texture2D) GD.Load(File.Rocket.FlameTexturePath);
		FlameSprite.Position = File.Rocket.flamePosition;
		FlameSprite.Offset = File.Rocket.flameOffset;
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
	
	public bool CheckComplete()
	{
		List<bool> complete = new List<bool>();
		CharacterBody2D finalBodyNode;
		
		if (File.Rocket.FinalBody == Game.CentreBody.Name)
			finalBodyNode = Game.CentreBody;
		else
			finalBodyNode = Game.Bodies[File.Rocket.FinalBody];
		
		if (File.Rocket.MinFinalAltitude != null)
			complete.Add((Position.DistanceTo(finalBodyNode.Position) - File.CentreBody.Radius >= File.Rocket.MinFinalAltitude));
		if (File.Rocket.MaxFinalAltitude != null)
			complete.Add((Position.DistanceTo(finalBodyNode.Position) - File.CentreBody.Radius <= File.Rocket.MaxFinalAltitude));
		if (File.Rocket.MinFinalSpeed != null)
			complete.Add(((finalBodyNode.Velocity - Velocity).Length() >= File.Rocket.MaxFinalSpeed));
		if (File.Rocket.MaxFinalSpeed != null)
			complete.Add(((finalBodyNode.Velocity - Velocity).Length() <= File.Rocket.MaxFinalSpeed));
		
		if (complete.Contains(false))
			return false;
		else
			return true;
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
