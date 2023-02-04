using Godot;
using System;

public class ShopButtonEnergy : Button{



	public override void _Ready(){
		OnRefresh();

		this.Connect("pressed",this,nameof(this.OnClick));
		Events.Instance.startedGame += OnStartedGame;
	}

	public override void _ExitTree(){
		Events.Instance.startedGame -= OnStartedGame;
	}

	public void OnStartedGame(){
		OnRefresh();
	}
	public void OnPurchase(){

		OnRefresh();
	}

	public void OnClick(){
		if(ResourcesManager.Instance.AttemptBuyEnergyLevel()){
			OnPurchase();
		}
	}

	public void OnRefresh(){
		if(ResourcesManager.Instance.IsCappedEnergyLevel()){
			Text = "Energy Level MAXED";
			Disabled = true;
			return;
		}
		Text = string.Format("Upgrade Energy (${0})",ResourcesManager.Instance.GetPriceOfEnergy());
		
		Disabled = false;
	}

}