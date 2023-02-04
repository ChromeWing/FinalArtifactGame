using Godot;
using System;
using ChromeWing.Temple;
public class ControlsManager : Singleton<ControlsManager>{

	private CameraPanning panning;

	private bool click = false;
	private bool pan = false;

	private IClickable target = null;

	private MinerNode targetNode{get{return target as MinerNode;}}
	private Vector2 clickedViewportOrigin,clickedWorldOrigin;

	public float LeftEdge {get{return panning.LimitLeft+2000;}}
	public float RightEdge {get{return panning.LimitRight-2000;}}
	public float TopEdge {get{return panning.LimitTop+1000;}}
	public float BottomEdge {get{return panning.LimitBottom-1000;}}

	public Vector2 WorldMousePos{get{return panning.GetGlobalMousePosition();}}

	private Vector2 deployPos = new Vector2(0,0);

	public int deployEnergyPrice {get{
		return 10+Mathf.RoundToInt(Mathf.Clamp(deployPos.y/10f,0,1000f)*Mathf.Clamp(deployPos.y/800f,1f,20f)/5f);
	}}


	public Vector2 GetDeployPos(){
		return deployPos;
	}

	public MinerNode GetTargetNode(){
		return targetNode;
	}

	public float DisplayWidth{get{return (int)ProjectSettings.GetSetting("display/window/size/width");}}
	public float DisplayHeight{get{return (int)ProjectSettings.GetSetting("display/window/size/height");}}

	public bool CursorOnToolbar{get{
		return ViewportPercent.y > .9f || (ViewportPercent.x>.95f && Mathf.Abs(ViewportPercent.y-.5f)<.2f);
	}}

	public Vector2 ViewportPercent{get{
		return new Vector2(panning.GetViewport().GetMousePosition().x/DisplayWidth,panning.GetViewport().GetMousePosition().y/DisplayHeight);
	}}
	
	public bool Panning{get{
		return pan && !ClickedToolbox;
	}}

	public bool ClickedToolbox{get{return clickedViewportOrigin.y > .9f;}}

	

	public ControlsManager(){
		panning = CameraPanning.Camera;
		panning.mouseMoveEvent += OnMouseMove;
		panning.mouseButtonEvent += OnMouseButton;

		Events.Instance.process += OnProcess;
	}

	public void OnProcess(float delta){
		//GD.Print(ViewportPercent);
		if(!GameManager.Instance.GameActive){return;}
		FindTarget();
		Events.Instance.deployCostChanged?.Invoke(deployEnergyPrice);
	}

	public void PanScreen(Vector2 move){
		if(!GameManager.Instance.GameActive){return;}
		panning.GlobalPosition += move;
		var pos = panning.GlobalPosition;
		GD.Print("rightEdge="+RightEdge+", we are="+pos.x);
		panning.GlobalPosition = new Vector2(
			Mathf.RoundToInt(Mathf.Clamp(pos.x,LeftEdge,RightEdge)),
			Mathf.RoundToInt(Mathf.Clamp(pos.y,TopEdge,BottomEdge))
			);
	}

	private void OnMouseMove(InputEventMouseMotion m){
		if(!GameManager.Instance.GameActive){return;}
		if(Panning){
			PanScreen(-m.Relative * 4f);
		}
	}

	private void OnMouseButton(InputEventMouseButton m){
		if(!GameManager.Instance.GameActive){return;}
		if(!click && m.IsActionPressed("click")){
			OnClicked();
		}
		if(!pan && m.IsActionPressed("pan")){
			OnClicked(false);
		}
		click = m.IsActionPressed("click");
		pan = m.IsActionPressed("pan");
	}

	private void OnClicked(bool leftClick=true){
		if(!GameManager.Instance.GameActive){return;}
		clickedViewportOrigin = ViewportPercent;
		clickedWorldOrigin = panning.GetGlobalMousePosition();
		
		if(leftClick){
			TryDeploy();
			if(!CursorOnToolbar){
				Events.Instance.clickedOffToolbar?.Invoke();
			}
		}
	}

	private void TryDeploy(){
		if(!GameManager.Instance.GameActive){return;}
		if(ClickedToolbox || CursorOnToolbar){return;}
		ResourcesManager.Instance.SpendEnergy(deployEnergyPrice,()=>{
			SceneManager.Instance.SpawnPrefab<MinerMiningNode>(SceneManager.Prefab.MinerNode,deployPos,target as Node).Init().Attach(targetNode);
		});
	}

	private void FindTarget(){
		if(!GameManager.Instance.GameActive){return;}
		var results = SpatialManager.Instance.FindClosestNonFindable(panning.GetGlobalMousePosition(),null,true);
		target = results.closest;
		deployPos = results.myPosition;
		if(deployPos.y<0){
			deployPos = new Vector2(deployPos.x,0);
		}
	}

	public void DisableAllRangeIndicators(){
		if(!GameManager.Instance.GameActive){return;}
		foreach(Node g in node.GetTree().GetNodesInGroup("MinerNode")){
			if(g is MinerNode){
				var miner = g as MinerNode;
				miner.SetRangeIndicator(false);
			}
		}
	}

}
