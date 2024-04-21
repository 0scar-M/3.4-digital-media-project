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
}
