using Godot;
using System;

public partial class LevelButton : Control
{
	public int LevelNum;
	
	public LevelButton() {}
	
	public void Init(int LevelNum)
	{
		// Change button properties for this instance
		this.LevelNum = LevelNum;
		this.Name = $"Level{LevelNum}";
		GetNode<Button>("Button").Text = $"  {LevelNum}  ";
	}
	
	private void OnButtonPressed()
	{
		// Connects from LevelButton instances
		GetNode<ScreenManager>("/root/ScreenManager").ChangeToScreen("GameScreen", this.LevelNum);
	}
}
