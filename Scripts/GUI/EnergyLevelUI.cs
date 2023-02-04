using Godot;
using System;

public class EnergyLevelUI : Label{

	public override void _Ready(){
		Events.Instance.energyLevelUpdated += OnUpdated;
		ResourcesManager.Instance.RefreshUI();
	}

	public override void _ExitTree(){
		Events.Instance.energyLevelUpdated -= OnUpdated;
	}

	public void OnUpdated(int energy){
		this.Text = string.Format("Lv. {0} Energy",energy);
	}

}