using Godot;
using System;

public partial class Levels : Node
{
	public LevelFile Level2 = new LevelFile(
		2, 
		"Orbit", 
		new CameraFile(new Vector2(0, 0), 0.5f), 
		new RocketFile(new Vector2(0, 0), 0f, new Vector2(0, 0), 2f, 50f, MinFinalSpeed:100f), 
		new CentreBodyFile(100f, 2f, "Earth", "res://Assests/earth.png", new Vector2(0, 1000))
	);
}
