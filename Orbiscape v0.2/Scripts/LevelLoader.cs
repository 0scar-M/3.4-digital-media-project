using Godot;
using System;

public partial class LevelLoader : Node2D
{
	public static void Load(int LevelNum)
	{
		// Add game nodes from LevelFile
		GD.Print($"Loading Level {LevelNum}...");
	}
}
