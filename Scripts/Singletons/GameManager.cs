using Godot;
using System;
using ChromeWing.Temple;

public class GameManager : Singleton<GameManager> {

	public bool GameActive{get;private set;}

	public GameManager(){
		Events.Instance.wonGame += OnWonGame;
	}

	public void OnWonGame(int time){
		GameActive=false;
	}

	public void StartNewGame(){
		foreach(var n in node.GetTree().GetNodesInGroup("MinerNode")){
			if(n is BaseNode){continue;}
			GameManager.Destroy(n as Node);
		}
		CameraPanning.Camera.GlobalPosition = Vector2.Zero;
		MapGenerator.Instance.Generate();
		ResourcesManager.Instance.Reset();
		GameActive = true;
		Events.Instance.startedGame?.Invoke();
	}


	public static void Destroy(Node node_){
		var parent = node_.GetParent();
		if(parent!=null){
			parent.RemoveChild(node_);
		}
		node_.QueueFree();
	}

	public static bool IsValid(Node node_){
		return node_!=null && Godot.Object.IsInstanceValid(node_) && node_.IsInsideTree();
	}

}