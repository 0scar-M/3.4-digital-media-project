using Godot;
using System;

public partial class Controls : Control
{
	private string rocketNodePath = "/root/GameScreen/Game/RocketNode";
	
	private void OnThrottleScrollBarValueChanged(double value)
	{
		GetNode<RocketNode>(rocketNodePath).ThrustLevel = (float)value;
	}
	
	private void OnRotateLeftButtonPressed()
	{
		GetNode<RocketNode>(rocketNodePath).RotationDegrees -= 10;
	}
	
	private void OnRotateRightButtonPressed()
	{
		GetNode<RocketNode>(rocketNodePath).RotationDegrees += 10;
	}
	
	private void OnThrustToggleButtonPressed()
	{
		// Input.ActionPress("ThrustToggle"); doesn't work for some reason, so I created a method for RocketNode that toggles the thrust.
		GetNode<RocketNode>(rocketNodePath).ToggleThrust();
	}
}
