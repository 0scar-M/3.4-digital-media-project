using Godot;
using System;

public partial class CameraNode : Camera2D
{
	public LevelFile File;
	
	public void Init()
	{
		Name = "Camera";
		Position = File.Camera.Centre;
		Zoom = new Vector2(File.Camera.Zoom, File.Camera.Zoom);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
	}
}
