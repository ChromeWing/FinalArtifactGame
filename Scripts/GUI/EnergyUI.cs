using Godot;
using System;

public class EnergyUI : Label{

	public override void _Ready(){
		Events.Instance.energyUpdated += OnEnergyUpdated;
		ResourcesManager.Instance.RefreshUI();
	}

	public override void _ExitTree(){
		Events.Instance.energyUpdated -= OnEnergyUpdated;
	}

	public void OnEnergyUpdated(int energy){
		this.Text = string.Format("{0} Energy",energy);
	}

}