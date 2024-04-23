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
	public float ThrustImpulse;
	
	public Sprite2D Sprite;
	public Sprite2D FlameSprite;
	public CollisionShape2D CollisionBody;
	public KinematicCollision2D Collision;
	public BaseBodyNode CurrentBody;
	
	private float _newThrustLevel;
	private float _scrollThrustIncrement = 0.05f;
	private float _keyThrustIncrement = 0.01f;
	private string _gameUIPath = "/root/GameScreen/GameUICanvas/GameUI/";
	private int _UIUpdateCount = 0;
	
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
		
		MotionMode = MotionModeEnum.Floating;
		Scale = ThisFile.Scale;
		Mass = ThisFile.Mass;
		ThrustImpulse = ThisFile.ThrustImpulse;
		Position = initNode.Position + _polarToCartesian(ThisFile.InitPosition.X - ((float) PI/2), initNode.Position.Length() + ThisFile.InitPosition.Y);
		Rotation = ThisFile.InitRotation;
		Velocity = ThisFile.InitVelocity;
		
		Sprite = GetNode<Sprite2D>("Sprite");
		Sprite.Texture = (Texture2D) GD.Load(ThisFile.RocketTexturePath);
		
		FlameSprite = GetNode<Sprite2D>("FlameSprite");
		FlameSprite.Texture = (Texture2D) GD.Load(ThisFile.FlameTexturePath);
		FlameSprite.Position = ThisFile.flamePosition;
		FlameSprite.Offset = ThisFile.flameOffset;
		FlameSprite.Hide();
		
		CollisionBody = GetNode<CollisionShape2D>("CollisionBody");
		var collisionShape = new RectangleShape2D();
		collisionShape.Size = new Vector2(Sprite.Texture.GetSize().X, Sprite.Texture.GetSize().Y);
		CollisionBody.Shape = collisionShape;
		
		CurrentBody = _findCurrentBody();
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
	
	public void ToggleThrust()
	{
		// Called by Input.IsActionJustPressed("ThrustToggle") and Controls.OnThrustToggleButtonPressed() 
		
		var thrustToggleButton = GetNode<Button>($"{_gameUIPath}Controls/ThrustToggleMargin/ThrustToggleButton");
		
		ThrustOn = !ThrustOn;
		if (ThrustOn)
			FlameSprite.Show();
		else
			FlameSprite.Hide();
		
		// Change thrust toggle button icon
		if (ThrustOn)
			thrustToggleButton.Icon = GD.Load<Texture2D>("res://Assests/Icons/pause.svg");
		else
			thrustToggleButton.Icon = GD.Load<Texture2D>("res://Assests/Icons/play.svg");
	}
	
	public override void _Ready()
	{
		// Stop physcis process from running so everything can initialise properly.
		SetPhysicsProcess(false);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("KeyRotateLeft"))
			RotationDegrees -= 1f;
		else if (Input.IsActionPressed("KeyRotateRight"))
			RotationDegrees += 1f;
		
		if (Input.IsActionPressed("KeyThrustUp"))
			ChangeThrust(true, _keyThrustIncrement);
		if (Input.IsActionPressed("KeyThrustDown"))
			ChangeThrust(false, _keyThrustIncrement);
		
		if (ThrustOn)
		{
			// Update Velocity based on Thrust
			Velocity += _polarToCartesian((float) (Rotation - PI/2), ThrustImpulse * ThrustLevel);
			FlameSprite.Scale = new Vector2(ThrustLevel, ThrustLevel);
		}
		
		// Update Velocity based on gravity. CentreBody:
		Velocity += _gravityAcceleration(Game.GravityConst, Position, Game.CentreBody.Position, Mass, Game.CentreBody.Mass);
		// Bodies:
		foreach (var b in Game.Bodies.Values)
			Velocity += _gravityAcceleration(Game.GravityConst, Position, b.Position, Mass, b.Mass);
		
		// Update position
		Collision = MoveAndCollide(Velocity * (float) delta);
		
		// Collision detection
		if (Collision != null)
		{
			if ((Velocity - Collision.GetColliderVelocity()).Length() >= File.Rocket.FatalCollisionSpeed)
				GetNode<MenuManager>($"{_gameUIPath}Menus").ChangeToMenu("FailMenu");
			
			// Bounce
			var reflect = Collision.GetRemainder().Bounce(Collision.GetNormal());
			Velocity = Velocity.Bounce(Collision.GetNormal());
			MoveAndCollide(reflect);
		}
		
		if (CheckComplete())
			GetNode<MenuManager>($"{_gameUIPath}Menus").ChangeToMenu("CompleteMenu");
		
		CurrentBody = _findCurrentBody();
		
		// Run _UIUpdateCount every 5 physics updates (60/5 = 12)
		if (_UIUpdateCount >= 12)
		{
			_updateUI();
			_UIUpdateCount = 0;
		}
		else
			_UIUpdateCount += 1;
	}
	
	public override void _Input (InputEvent input)
	{
		if (Input.IsActionJustPressed("ThrustToggle"))
			ToggleThrust();
		
		if (Input.IsActionPressed("ScrollThrustUp"))
			ChangeThrust(true, _scrollThrustIncrement);
		if (Input.IsActionPressed("ScrollThrustDown"))
			ChangeThrust(false, _scrollThrustIncrement);
	}
	
	private Vector2 _polarToCartesian(float theta, float radius)
	{
		return new Vector2((float) (radius * Cos(theta)), (float) (radius * Sin(theta)));
	}
	
	private Vector2 _gravityAcceleration(float gravityConst, Vector2 objectPosition, Vector2 bodyPosition, float objectMass, float bodyMass)
	{
		return _polarToCartesian(objectPosition.AngleToPoint(bodyPosition), gravityConst * objectMass * objectMass * bodyMass / objectPosition.DistanceTo(bodyPosition));
	}
	
	private BaseBodyNode _findCurrentBody()
	{
		// Find which body's sphere of influence the rocket is in.
		BaseBodyNode newCurrentBody = null;
		if (Game.Bodies.Count != 0)
		{
			foreach (var body in Game.Bodies.Values)
			{
				if (Position.DistanceTo(body.Position) <= body.InfluenceRadius)
					if (newCurrentBody != null) 
						if (_gravityAcceleration(Game.GravityConst, Position, body.Position, Mass, body.Mass) < _gravityAcceleration(Game.GravityConst, Position, newCurrentBody.Position, Mass, newCurrentBody.Mass))
						// If the rocket is more attracted to body rather than newCurrentBody, replace newCurrentBody with body.
							newCurrentBody = body;
					else
						newCurrentBody = body;
			}
			if (newCurrentBody == null)
				newCurrentBody = Game.CentreBody;
		}
		else
			newCurrentBody = Game.CentreBody;
		
		return newCurrentBody;
	}
	
	private void ChangeThrust(bool isUp, float increment)
	{
		if (isUp)
			_newThrustLevel = ThrustLevel + increment;
		else
			_newThrustLevel = ThrustLevel - increment;
		
		if (_newThrustLevel >= 1)
			_newThrustLevel = 1;
		if (_newThrustLevel <= 0)
			_newThrustLevel = 0;
		
		ThrustLevel = _newThrustLevel;
		
		// Update thrust slider and label
		GetNode<VScrollBar>($"{_gameUIPath}Controls/ThrustMargin/Thrust/ThrustScrollBar").Value = ThrustLevel;
		GetNode<Label>($"{_gameUIPath}Controls/ThrustLabelMargin/ThrustLabel").Text = $"{Math.Round(100*ThrustLevel, 0)}%";
	}
	
	private void _updateUI()
	{
		GetNode<Label>($"{_gameUIPath}Controls/ReadoutsMargin/ReadoutsVBox/AltitudeMargin/AltitudeLabel").Text = $"Altitude: {Math.Round(Position.DistanceTo(CurrentBody.Position) - CurrentBody.Radius, 0)}m ({CurrentBody.Name})";
		GetNode<Label>($"{_gameUIPath}Controls/ReadoutsMargin/ReadoutsVBox/SpeedMargin/SpeedLabel").Text = $"Speed: {Math.Round((Velocity - CurrentBody.Velocity).Length(), 0)}m/s ({CurrentBody.Name})";
		
		int heading;
		if (RotationDegrees >= 0)
			heading = (int) Math.Round(RotationDegrees, 0);
		else
			heading = (int) Math.Round(RotationDegrees + 360, 0);
			
		GetNode<Label>($"{_gameUIPath}Controls/RotationMargin/VBoxContainer/LabelMargin/Label").Text = $"Heading: {heading}°";
	}
}
