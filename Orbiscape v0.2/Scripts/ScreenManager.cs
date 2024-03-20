using Godot;
using System;

public partial class ScreenManager : Node
{	
	public override void _Ready()
	{
		string initialScreen = "StartScreen";
		ChangeToScreen(initialScreen);
	}
	
	public void ChangeToScreen(string targetScreen, int targetLevel = 0)
	{
		// Wait one frame before changing screen
		CallDeferred(nameof(DeferredChangeToScreen), targetScreen, targetLevel);
	}
	
	private void DeferredChangeToScreen(string targetScreen, int targetLevel = 0)
	{
		string screenFolderPath = "res://scenes/screens/";
		GetTree().ChangeSceneToFile($"{screenFolderPath}{targetScreen}.tscn");
		
		if (targetScreen == "GameScreen" && targetLevel != 0)
		{
			LevelLoader.Load(targetLevel);
		}
	}
}
