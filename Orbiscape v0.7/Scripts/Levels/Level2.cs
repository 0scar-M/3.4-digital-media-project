using Godot;
using System;

public partial class Levels : Node
{
	public LevelFile Level2 = new LevelFile(
		Num: 2, 
		Goal: "Complete one orbit above an altitude of 250m.", 
		GravityConst: 500f, 
		
		new CameraFile(
			Centre: new Vector2(0, 0), 
			Zoom: 0.5f
		), 
		new RocketFile(
			InitBody: "Earth", 
			InitPosition: new Vector2(0, 0), 
			InitRotation: 0, 
			InitVelocity: new Vector2(0, 0), 
			Mass: 0.1f, 
			Scale: 0.03f, 
			ThrustImpulse: 0.5f, 
			FinalBody: "Earth", 
			MinFinalAltitude: 250f, 
			MinFinalSpeed: 90f, // 90 < 122.5 in case of elliptical orbits
			MinFinalTime: 38.5f
			//Orbit velocity @ 250m = 122.5m/s, Period = 38.5
		), 
		new CentreBodyFile(
			Name: "Earth", 
			InitPosition: new Vector2(0, 0), 
			Radius: 500f, 
			Mass: 50f, 
			TexturePath: "res://Assests/earth.png", 
			TextureRadius: 0.8f
		)
	);
}
