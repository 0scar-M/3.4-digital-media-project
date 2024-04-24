using Godot;
using System;
using System.Collections.Generic;

public partial class MenuManager : Control
{
	private Dictionary<string, Menu> _menus = new Dictionary<string, Menu>();
	private string _initialMenu = "NullMenu";
	public Menu _currentMenu;
	
	public override void _Ready()
	{
		foreach (var node in GetChildren())
		{
			if (node is Menu m)
			{
				_menus[node.Name] = m;
				m.manager = this;
				m.Exit(); // reset
			}
		}
		
		_currentMenu = GetNode<Menu>(_initialMenu);
		ChangeToMenu(_currentMenu.Name);
	}
	
	public void ChangeToMenu(string name)
	{
		if (!_menus.ContainsKey(name) || _menus[name] == _currentMenu)
			return;
		
		_currentMenu.Exit();
		_currentMenu = _menus[name];
		_currentMenu.Enter();
	}
}
