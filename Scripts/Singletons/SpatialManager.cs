using Godot;
using System;
using ChromeWing.Temple;
public class SpatialManager : Singleton<SpatialManager>{



	public SpatialManager(){

	}

	public IClickable FindMinerNodeInClickableRange(Vector2 worldPos){
		IClickable target = null;
		var pos = worldPos;
		var bestRange = -1f;
		foreach(Node g in node.GetTree().GetNodesInGroup("MinerNode")){
			if(g is IClickable){
				var clickable = g as IClickable;
				if(!clickable.IsTargetable()){continue;}
				var range = clickable.GetRange(pos);
				if(clickable.InClickRange(pos)){
					if(bestRange<0 || range<bestRange){
						bestRange = range;
						target = clickable;
					}
				}
			}
		}
		return target;
	}

	public ClosestNonFindableResults FindClosestNonFindable(Vector2 pos,MinerNode from=null,bool stretchToFit=false){
		MinerNode target = null;
		var bestRange = -1f;
		foreach(Node g in node.GetTree().GetNodesInGroup("MinerNode")){
			if(from!=null && from==g){continue;}
			if(g is MinerNode && !(g is IFindable)){
				var miner = g as MinerNode;
				var range = miner.GetRange(pos);
				if(stretchToFit){
					if(bestRange<0 || range<bestRange){
						bestRange = range;
						target = miner;
					}
					continue;
				}
				if(miner.InHarvestableRange(pos)){
					if(bestRange<0 || range<bestRange){
						bestRange = range;
						target = miner;
					}
				}
			}
		}

		//GD.Print("closest unit to us is "+target.Name);
		if(stretchToFit && target!=null && (pos-target.GlobalPosition).Length() > target.GetHarvestableRange()){
			pos = (target.GlobalPosition+(pos-target.GlobalPosition).Normalized()*target.GetHarvestableRange());
		}

		return new ClosestNonFindableResults(target,target!=null,pos);
	}

}

public struct ClosestNonFindableResults{
	public MinerNode closest;
	public bool valid;
	public Vector2 myPosition;

	public ClosestNonFindableResults(MinerNode closest,bool valid,Vector2 myPosition){
		this.closest = closest;
		this.valid = valid;
		this.myPosition = myPosition;
	}
}