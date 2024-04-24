using Godot;
using System;

public partial class GameUI : Control
{
	public void OnPauseButtonPressed()
	{
		GetNode<MenuManager>("Menus").ChangeToMenu("PauseMenu");
	}
	
	public void OnUnpauseButtonPressed()
	{
		GetNode<MenuManager>("Menus").ChangeToMenu("NullMenu");
	}
	
	public void OnNextLevelButtonPressed()
	{
		GetNode<MenuManager>("Menus").ChangeToMenu("NullMenu");
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("GameScreen", targetLevelFile: Levels.LevelFiles[GetNode<GameNode>("/root/GameScreen/Game").File.Num + 1]);
	}
	
	public void OnRestartButtonPressed()
	{
		GetNode<MenuManager>("Menus").ChangeToMenu("NullMenu");
		// Reload game scene
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("GameScreen", targetLevelFile: GetNode<GameNode>("/root/GameScreen/Game").File);
	}
	
	public void OnExitButtonPressed()
	{
		GetNode<MenuManager>("Menus").ChangeToMenu("NullMenu");
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("LevelSelectScreen");
	}
	
	public override void _Input (InputEvent input)
	{
		if (Input.IsActionJustPressed("PauseToggle"))
		{
			if (GetTree().Paused)
				OnUnpauseButtonPressed();
			else
				OnPauseButtonPressed();
		}
		
		if (Input.IsActionJustPressed("Restart"))
			OnRestartButtonPressed();
		
		if (Input.IsActionJustPressed("Exit"))
			OnExitButtonPressed();
	}

}
