using Godot;
using System;

public class DeployCursorGuide : Sprite {

	private MinerNode lastTarget;

	public override void _Process(float delta){
		this.Visible = GameManager.Instance.GameActive;
		if(lastTarget!=ControlsManager.Instance.GetTargetNode()){
			RefreshTarget(ControlsManager.Instance.GetTargetNode());
		}
		this.GlobalPosition = ControlsManager.Instance.GetDeployPos();
	}

	private void RefreshTarget(MinerNode target){
		lastTarget = target;

		ControlsManager.Instance.DisableAllRangeIndicators();
		lastTarget.SetRangeIndicator(true);
	}

}