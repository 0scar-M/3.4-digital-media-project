using Godot;
using System;
using static LevelFile;

public partial class Game : Node2D
{
	public CameraNode Camera;
	public RocketNode Rocket;
	public CentreBodyNode CentreBody;
	public Node Bodies;
	
	public override void _Ready()
	{
		LevelFile File = GetNode<ScreenManager>("/root/ScreenManager").currentLevelFile;
		
		// Add Camera Node
		this.AddChild(new Camera2D());
		GetChild<Camera2D>(0).SetScript(GD.Load(File.Camera.ScriptPath));
		Camera = GetChild<CameraNode>(0);
		Camera.File = File;
		Camera.Init();
		
		// Add Rocket Node
		this.AddChild(new CharacterBody2D());
		GetChild<CharacterBody2D>(1).SetScript(GD.Load(File.Rocket.ScriptPath));
		Rocket = GetChild<RocketNode>(1);
		Rocket.File = File;
		Rocket.Init();
		
		// Add CentreBody Node
		this.AddChild(new CharacterBody2D());
		GetChild<CharacterBody2D>(2).SetScript(GD.Load(File.CentreBody.ScriptPath));
		CentreBody = GetChild<CentreBodyNode>(2);
		CentreBody.File = File;
		CentreBody.Init();
		
		// Add Body Nodes
		this.AddChild(new Node());
		Bodies = this.GetChild<Node>(3);
		Bodies.Name = "Bodies";
		
		foreach (var Body in File.Bodies)
		{
			Bodies.AddChild(new CharacterBody2D());
			GetChild<CharacterBody2D>(0).SetScript(GD.Load(Body.ScriptPath));
			var body = GetChild<BodyNode>(0);
			body.File = Body;
			body.Init();
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
	}
}
