using Godot;
using System;

public partial class StartScreen : Control
{
	private void OnStartButtonPressed()
	{
		// Connects from MenuCenter/MenuVBox/ButtonsVBox/StartButton
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("LevelSelectScreen");
	}
	
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
	
	public override void _Input (InputEvent input)
	{
		if (Input.IsActionJustPressed("PauseToggle"))
			OnQuitButtonPressed();
	}
}
