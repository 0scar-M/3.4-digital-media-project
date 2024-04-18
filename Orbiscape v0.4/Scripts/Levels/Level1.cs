using Godot;
using System;

public partial class Levels : Node
{
	public LevelFile Level1 = new LevelFile(
		Num: 1, 
		Name: "Flight", 
		GravityConst: 5f, 
		
		new CameraFile(
			Centre: new Vector2(0, 0), 
			Zoom: 0.5f
		), 
		new RocketFile(
			InitBody: "Earth", 
			InitPosition: new Vector2(0, 0), 
			InitRotation: 0f, 
			InitVelocity: new Vector2(0, 0), 
			Mass: 1f, 
			Scale: 0.03f, 
			ThrustImpulse: 0.8f, 
			FinalBody: "Earth", 
			MinFinalAltitude:  500f
		), 
		new CentreBodyFile(
			Name: "Earth", 
			InitPosition: new Vector2(0, 1000), 
			Radius: 1000f, 
			Mass: 100f, 
			TexturePath: "res://Assests/earth.png", 
			TextureRadius: 0.8f
		)
	);
}
