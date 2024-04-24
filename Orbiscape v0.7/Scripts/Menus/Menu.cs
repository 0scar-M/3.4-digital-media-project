using Godot;
using System;

public partial class Menu : Control
{
	public MenuManager manager;
	
	public virtual void Enter()
	{
		GetTree().Paused = true;
		// Move menus node to bottom so it is able to be interacted with.
		GetParent().GetParent().CallDeferred("move_child", GetParent(), 1);
		Show();
	}
	
	public virtual void Exit()
	{
		Hide();
		// Move menus node to top so controls is able to be interacted with.
		GetParent().GetParent().CallDeferred("move_child", GetParent(), 0);
		GetTree().Paused = false;
	}
}
