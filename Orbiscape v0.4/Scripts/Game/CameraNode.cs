using Godot;
using System;

public partial class CameraNode : Camera2D
{
	public LevelFile File;
	public CameraFile ThisFile;
	public GameNode Game;
	
	public void Init(LevelFile parsedFile, CameraFile parsedThisFile, GameNode parsedGame)
	{
		/*
		Sets instance-specific node data. 
		_Ready() doesn't work for this because properties might depend on other nodes that haven't been created yet.
		*/
		
		this.File = parsedFile;
		this.ThisFile = parsedThisFile;
		this.Game = parsedGame;
		
		Position = ThisFile.Centre;
		Zoom = new Vector2(ThisFile.Zoom, ThisFile.Zoom);
	}
	
	public override void _Ready()
	{
		// Stop physcis process from running so everything can initialise properly.
		SetPhysicsProcess(false);
	}
}
