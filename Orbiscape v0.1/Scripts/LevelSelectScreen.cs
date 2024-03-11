using Godot;
using System;

public partial class LevelSelectScreen : Control
{
	private void OnBackButtonPressed()
	{
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("StartScreen");
	}
}
