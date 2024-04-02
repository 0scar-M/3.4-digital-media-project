#nullable enable

using Godot;
using System;

public partial class LevelFile : Object
{
	public int Num;
	public string Title;
	
	public CameraFile Camera;
	public RocketFile Rocket;
	public CentreBodyFile CentreBody;
	public BodyFile[] Bodies;
	
	public LevelFile(int Num, string Title, CameraFile Camera, RocketFile Rocket, CentreBodyFile CentreBody, params BodyFile[] Bodies)
	{
		this.Num = Num;
		this.Title = Title;
		this.Camera = Camera;
		this.Rocket = Rocket;
		this.CentreBody = CentreBody;
		this.Bodies = Bodies;
		
		// Add LevelFile to LevelFiles dict.
		Levels.LevelFiles[this.Num] = this;
	}
}

public partial class CameraFile : Object
{
	public Vector2 Centre;
	public float Zoom;
	
	public string ScriptPath = "res://Scripts/Game/CameraNode.cs";
	
	public CameraFile(Vector2 Centre, float Zoom)
	{
		this.Centre = Centre;
		this.Zoom = Zoom;
	}
}

public partial class RocketFile : Object
{
	public Vector2 InitPosition;
	public float InitRotation;
	public Vector2 InitVelocity;
	public float ThrustRate;
	public float Scale;
	
	// Final position/speeds to check if level completed. 
	// Nullable so they are optional in a level definition.
	public Rect2? FinalPosition;
	public float? MinFinalSpeed;
	public float? MaxFinalSpeed;
	
	public string ScriptPath = "res://Scripts/Game/RocketNode.cs";
	public string RocketTexturePath = "res://Assests/rocket.png";
	public string FlameTexturePath = "res://Assests/flame.png";
	
	public RocketFile(Vector2 InitPosition, float InitRotation, Vector2 InitVelocity, float ThrustRate, float Scale, Rect2? FinalPosition = null, float? MinFinalSpeed = null, float? MaxFinalSpeed = null)
	{
		this.InitPosition = InitPosition;
		this.InitRotation = InitRotation;
		this.InitVelocity = InitVelocity;
		this.ThrustRate = ThrustRate;
		this.Scale = Scale;
		this.FinalPosition = FinalPosition;
		this.MinFinalSpeed = MinFinalSpeed;
		this.MaxFinalSpeed = MaxFinalSpeed;
	}
}

public abstract partial class BaseBodyFile : Object
{
	public float Mass;
	public float Radius;
	public Vector2 InitPosition;
	
	public string Name;
	public string TexturePath;
	
	public BaseBodyFile(float Mass, float Radius, string Name, string TexturePath, Vector2 InitPosition)
	{
		this.Mass = Mass;
		this.Radius = Radius;
		this.Name = Name;
		this.TexturePath = TexturePath;
		this.InitPosition = InitPosition;
	}
}

public partial class BodyFile : BaseBodyFile
{
	public float OrbitRadius;
	public string ScriptPath = "res://Scripts/Game/BodyNode.cs";
	
	public BodyFile(float Mass, float Radius, string Name, string TexturePath, float OrbitRadius, Vector2 InitPosition) : base(Mass, Radius, Name, TexturePath, InitPosition)
	{
		this.OrbitRadius = OrbitRadius;
	}
}

public partial class CentreBodyFile : BaseBodyFile
{
	public string ScriptPath = "res://Scripts/Game/CentreBodyNode.cs";
	
	public CentreBodyFile(float Mass, float Radius, string Name, string TexturePath, Vector2 InitPosition) : base(Mass, Radius, Name, TexturePath, InitPosition) {}
}
