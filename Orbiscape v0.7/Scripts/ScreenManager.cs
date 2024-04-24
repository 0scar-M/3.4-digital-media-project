#nullable enable

using Godot;
using System;

public partial class ScreenManager : Node
{
	[Export] public string initialScreen = "StartScreen";
	
	private string? _targetScreen;
	public LevelFile? currentLevelFile;
	
	public override void _Ready()
	{
		ChangeToScreen(initialScreen);
	}
	
	public void ChangeToScreen(string targetScreen, LevelFile? targetLevelFile = null)
	{
		_targetScreen = targetScreen;
		currentLevelFile = targetLevelFile;
		// Wait one frame before changing screen
		CallDeferred(nameof(DeferredChangeToScreen));
	}
	
	private void DeferredChangeToScreen()
	{
		string screenFolderPath = "res://scenes/screens/";
		GetTree().ChangeSceneToFile($"{screenFolderPath}{_targetScreen}.tscn");
	}
}
