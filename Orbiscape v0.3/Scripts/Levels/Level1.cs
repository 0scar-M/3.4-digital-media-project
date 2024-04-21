using Godot;
using System;

public partial class Levels : Node
{
	public LevelFile Level1 = new LevelFile(
		1, 
		"Flight", 
		new CameraFile(new Vector2(0, 0), 1), 
		new RocketFile(new Vector2(0, -100), 0f, new Vector2(0, 0), 1f, 0.03f, FinalPosition:new Rect2(0f, 0f, 1920f, 100f)), 
		new CentreBodyFile(100f, 2f, "Earth", "res://Assests/earth.png", new Vector2(0, 1000))
	);
}
