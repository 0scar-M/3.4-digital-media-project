using Godot;
using System;

public partial class PauseMenu : Control
{
	public override void _Ready()
	{
		Hide();
	}
	
	private void OnPauseButtonPressed()
	{
		GetTree().Paused = true;
		Show();
	}
	
	private void OnUnpauseButtonPressed()
	{
		Hide();
		GetTree().Paused = false;
	}
	
	public void OnRestartButtonPressed()
	{
		GetTree().Paused = false;
		// Reload game scene
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("GameScreen", targetLevelFile: GetNode<GameNode>("/root/GameScreen/Game").File);
	}
	
	public void OnExitButtonPressed()
	{
		GetTree().Paused = false;
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("LevelSelectScreen");
	}
}
