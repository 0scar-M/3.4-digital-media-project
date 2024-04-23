using Godot;
using System;

public partial class NullMenu : Menu
{
	public override void Enter()
	{
		//GetParent<Control>().ZIndex = -1;
		GetParent().GetParent().CallDeferred("move_child", GetParent(), 0);
	}
	
	public override void Exit()
	{
		//GetParent<Control>().ZIndex = 1;
		GetParent().GetParent().CallDeferred("move_child", GetParent(), 0);
	}
}
