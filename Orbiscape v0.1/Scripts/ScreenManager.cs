using Godot;
using System;
using System.Collections.Generic;

public partial class ScreenManager : Node
{	
	public override void _Ready()
	{
		string initialScreen = "StartScreen";
		ChangeToScreen(initialScreen);
	}
	
	public void ChangeToScreen(string targetScreen)
	{
		string screenFolderPath = "res://Scenes/Screens/";
		GetTree().ChangeSceneToFile($"{screenFolderPath}{targetScreen}.tscn");
	}
}
