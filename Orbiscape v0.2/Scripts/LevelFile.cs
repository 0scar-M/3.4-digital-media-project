using Godot;
using System;

public partial class LevelFile : Object
{
	public int Num;
	
	public LevelFile(int Num)
	{
		this.Num = Num;
		
		// Add LevelFile to LevelFiles dict.
		Levels.LevelFiles[this.Num] = this;
	}
}
