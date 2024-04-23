using Godot;
using System;

public partial class LevelSelectScreen : Control
{
	public override void _Ready()
	{
		// Unpause
		GetTree().Paused = false;
		
		Levels LevelsFile = new Levels();
		
		// Add LevelButton scene instances in grid format
		foreach (var l in Levels.LevelFiles.Keys)
		{
			var button = (LevelButton)GD.Load<PackedScene>("res://scenes/buttons/LevelButton.tscn").Instantiate();
			button.Init(l);
			GetNode("LevelsCenter/LevelsMargin/LevelsGrid").AddChild(button);
		}
	}
	
	private void OnBackButtonPressed()
	{
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("StartScreen");
	}
}
