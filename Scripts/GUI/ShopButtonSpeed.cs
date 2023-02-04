using Godot;
using System;

public class ShopButtonSpeed : Button{



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
		if(ResourcesManager.Instance.AttemptBuySpeedLevel()){
			OnPurchase();
		}
	}

	public void OnRefresh(){
		if(ResourcesManager.Instance.IsCappedSpeedLevel()){
			Text = "Speed Level MAXED";
			Disabled = true;
			return;
		}
		Text = string.Format("Upgrade Speed (${0})",ResourcesManager.Instance.GetPriceOfSpeed());
		
		Disabled = false;
	}

}