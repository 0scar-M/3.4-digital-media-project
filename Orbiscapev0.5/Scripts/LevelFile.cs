#nullable enable

using Godot;
using System;
using System.Collections.Generic;

public partial class LevelFile : Object
{
	public int Num;
	public string Name;
	
	public float GravityConst;
	
	public CameraFile Camera;
	public RocketFile Rocket;
	public CentreBodyFile CentreBody;
	public BodyFile[] Bodies;
	
	public LevelFile(int Num, string Name, float GravityConst, CameraFile Camera, RocketFile Rocket, CentreBodyFile CentreBody, params BodyFile[] Bodies)
	{
		this.Num = Num;
		this.Name = Name;
		this.GravityConst = GravityConst;
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
	public string InitBody;
	public Vector2 InitPosition; // Polar coordinates (longitude, altitude) relative to InitBody.
	public float InitRotation;
	public Vector2 InitVelocity;
	public float Mass;
	public Vector2 Scale;
	public float ThrustImpulse;
	
	// Body that final values are relative to.
	public string FinalBody;
	// Values to be checked to see if level completed. Nullable so they are optional in a level definition.
	public float? MinFinalAltitude;
	public float? MaxFinalAltitude;
	public float? MinFinalSpeed;
	public float? MaxFinalSpeed;
	
	public string ScriptPath = "res://Scripts/Game/RocketNode.cs";
	public string RocketTexturePath = "res://Assests/rocket.png";
	public string FlameTexturePath = "res://Assests/flame.png";
	public Vector2 flamePosition = new Vector2(-25, 1200);
	public Vector2 flameOffset = new Vector2(0, 600);
	public float FatalCollisionSpeed = 100;
	
	public RocketFile(string InitBody, Vector2 InitPosition, float InitRotation, Vector2 InitVelocity, float Mass, float Scale, float ThrustImpulse, string FinalBody, float? MinFinalAltitude = null, float? MaxFinalAltitude = null, float? MinFinalSpeed = null, float? MaxFinalSpeed = null)
	
	{
		this.InitBody = InitBody;
		this.InitPosition = InitPosition;
		this.InitRotation = InitRotation;
		this.InitVelocity = InitVelocity;
		this.Mass = Mass;
		this.Scale = new Vector2(Scale, Scale);
		this.ThrustImpulse = ThrustImpulse;
		this.FinalBody = FinalBody;
		this.MinFinalAltitude = MinFinalAltitude;
		this.MaxFinalAltitude = MaxFinalAltitude;
		this.MinFinalSpeed = MinFinalSpeed;
		this.MaxFinalSpeed = MaxFinalSpeed;
	}
}

public abstract partial class BaseBodyFile : Object
{
	public string Name;
	public string TexturePath;
	public float TextureRadius;
	
	public float Radius;
	public float Mass;
	
	public BaseBodyFile(string Name, string TexturePath, float TextureRadius, float Radius, float Mass)
	{
		this.Name = Name;
		this.TexturePath = TexturePath;
		this.TextureRadius = TextureRadius;
		this.Radius = Radius;
		this.Mass = Mass;
	}
}

public partial class BodyFile : BaseBodyFile
{
	public float OrbitRadius;
	public float InitOrbitAngle;
	public bool IsOrbitClockwise;
	public string ScriptPath = "res://Scripts/Game/BodyNode.cs";
	
	public BodyFile(string Name, string TexturePath, float TextureRadius, float Radius, float Mass, float OrbitRadius, float InitOrbitAngle, bool IsOrbitClockwise) : base(Name, TexturePath, TextureRadius, Radius, Mass)
	{
		this.OrbitRadius = OrbitRadius;
		this.InitOrbitAngle = InitOrbitAngle;
		this.IsOrbitClockwise = IsOrbitClockwise;
	}
}

public partial class CentreBodyFile : BaseBodyFile
{
	public Vector2 InitPosition;
	public string ScriptPath = "res://Scripts/Game/CentreBodyNode.cs";
	
	public CentreBodyFile(string Name, Vector2 InitPosition, float Radius, float Mass, string TexturePath, float TextureRadius) : base(Name, TexturePath, TextureRadius, Radius, Mass)
	{
		this.InitPosition = InitPosition;
	}
}
