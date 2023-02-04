using Godot;
using System;

public class MoneyUI : Label{

	public override void _Ready(){
		Events.Instance.moneyUpdated += OnMoneyUpdated;
		ResourcesManager.Instance.RefreshUI();
	}

	public override void _ExitTree(){
		Events.Instance.moneyUpdated -= OnMoneyUpdated;
	}

	public void OnMoneyUpdated(int money){
		this.Text = string.Format("${0}",money);
	}

}