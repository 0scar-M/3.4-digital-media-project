using Godot;
using System;
public partial class LevelSelectScreen : Control
{
	public override void _Ready()
	{
		Levels LevelsFile = new Levels();
		int buttonsPerRow = 5;
		
		// Add LevelButton scene instances in grid format
		for (int x = 0; x <= Math.Floor((float)Levels.LevelFiles.Count / buttonsPerRow); ++x)
		{
			// Add HBoxContainer row
			HBoxContainer row = new HBoxContainer();
			row.Name = $"Row{x}";
			GetNode("LevelsCenter/LevelsMargin/LevelsColumns").AddChild(row);
			
			for (int y = (buttonsPerRow*x)+1; y <= buttonsPerRow*(x+1) && y <= Levels.LevelFiles.Count; ++y)
			{
				// Add LevelButton instance as child of row
				var button = (LevelButton)GD.Load<PackedScene>("res://scenes/buttons/LevelButton.tscn").Instantiate();
				button.Init(y);
				row.AddChild(button);
			}
		}
	}
	
	private void OnBackButtonPressed()
	{
		// Connects from BackButtonMargin/BackButton
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("StartScreen");
	}
}
