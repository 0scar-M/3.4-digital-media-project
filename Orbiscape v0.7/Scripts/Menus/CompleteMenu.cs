using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CompleteMenu : Menu
{
	public override void Enter()
	{
		GetTree().Paused = true;
		
		int highestLevel = Levels.LevelFiles.Aggregate((x, y) => x.Key > y.Key ? x : y).Key;
		// Disable NextLevelButton if on last level.
		if (GetNode<GameNode>("/root/GameScreen/Game").File.Num >= highestLevel)
			GetNode<Button>("MenuCenter/MenuVBox/ButtonsVBox/NextLevelButton").Disabled = true;
		
		// Move Menus node to bottom so it is able to be interacted with.
		GetParent().GetParent().CallDeferred("move_child", GetParent(), 1);
		Show();
	}
}
