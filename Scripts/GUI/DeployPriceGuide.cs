using Godot;
using System;

public class DeployPriceGuide : Label {

	public override void _Ready(){
		Events.Instance.deployCostChanged += UpdatePrice;
	}

	public override void _ExitTree(){
		Events.Instance.deployCostChanged -= UpdatePrice;
	}

	public void UpdatePrice(int price){
		Text = string.Format("building costs {0} energy",price);
	}

}