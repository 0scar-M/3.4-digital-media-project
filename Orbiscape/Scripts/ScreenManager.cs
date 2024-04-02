#nullable enable

using Godot;
using System;

public partial class ScreenManager : Node
{	
	private string? _targetScreen;
	public LevelFile? currentLevelFile;
	
	public override void _Ready()
	{
		string initialScreen = "StartScreen";
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
		
		//if (_targetScreen == "GameScreen" && _targetLevelFile != null)
		//{
			//GetNode<Game>("/root/GameScreen/Game").BuildScene(_targetLevelFile);
		//}
	}
}
