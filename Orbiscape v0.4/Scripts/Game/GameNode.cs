using Godot;
using System;
using System.Collections.Generic;
using static LevelFile;

public partial class GameNode : Node2D
{
	public LevelFile File;
	
	public CameraNode Camera;
	public RocketNode Rocket;
	public CentreBodyNode CentreBody;
	public Dictionary<string, BodyNode> Bodies = new Dictionary<string, BodyNode>();
	public float GravityConst;
	
	private int _count = 0;
	
	public override void _Ready()
	{
		File = GetNode<ScreenManager>("/root/ScreenManager").currentLevelFile;
		GravityConst = File.GravityConst;
		
		// Add Camera Node
		AddChild((CameraNode) GD.Load<PackedScene>("res://Scenes/GameNodes/CameraNode.tscn").Instantiate());
		Camera = GetNode<CameraNode>("CameraNode");
		Camera.Init(File, File.Camera, this);
		
		// CentreBody Node
		AddChild((CentreBodyNode) GD.Load<PackedScene>("res://Scenes/GameNodes/CentreBodyNode.tscn").Instantiate());
		CentreBody = GetNode<CentreBodyNode>("CentreBodyNode");
		CentreBody.Init(File, File.CentreBody, this);
		
		// Add Body Nodes
		foreach (var Body in File.Bodies)
		{
			AddChild((BodyNode) GD.Load<PackedScene>("res://Scenes/GameNodes/BodyNode.tscn").Instantiate());
			var body = GetChild<BodyNode>(GetChildCount()-1);
			body.Init(File, Body, this);
			
			Bodies.Add(body.Name, body);
		}
		
		// Add Rocket Node 
		// Setup last because its initial Position depends on other bodies.
		AddChild((RocketNode) GD.Load<PackedScene>("res://Scenes/GameNodes/RocketNode.tscn").Instantiate());
		Rocket = GetNode<RocketNode>("RocketNode");
		Rocket.Init(File, File.Rocket, this);
		
		SetPhysicsProcess(true);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// Wait one physics frame before running children's physics processes so weird stuff doesn't happen.
		if (_count >= 1)
		{
			Camera.SetPhysicsProcess(true);
			Rocket.SetPhysicsProcess(true);
			CentreBody.SetPhysicsProcess(true);
			foreach (var body in Bodies.Values)
			{
				body.SetPhysicsProcess(true);
			}
			
			// Stop this script because it doesn't need to run.
			SetPhysicsProcess(false);
		}
		
		_count += 1;
	}
}
