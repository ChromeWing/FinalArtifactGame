using Godot;
using System;

public class SpeedLevelUI : Label{

	public override void _Ready(){
		Events.Instance.speedLevelUpdated += OnUpdated;
		ResourcesManager.Instance.RefreshUI();
	}

	public override void _ExitTree(){
		Events.Instance.speedLevelUpdated -= OnUpdated;
	}

	public void OnUpdated(int speed){
		this.Text = string.Format("Lv. {0} Speed",speed);
	}

}