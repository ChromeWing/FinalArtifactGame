using Godot;
using ChromeWing.Temple;
using System.Collections.Generic;

public class SceneManager : Singleton<SceneManager> {



	public Dictionary<SceneManager.Prefab,PackedScene> prefabs;
	public Dictionary<SceneManager.UI,PackedScene> ui;


	public SceneManager(){
		PreloadPrefabs();
		PreloadUI();
	}

	private void PreloadPrefabs(){
		prefabs = new Dictionary<Prefab, PackedScene>();
		prefabs.Add(Prefab.MinerNode,GD.Load("res://Prefabs/Nodes/MinerNode.tscn") as PackedScene);

		
		prefabs.Add(Prefab.Battery,GD.Load("res://Prefabs/Nodes/Battery.tscn") as PackedScene);
		prefabs.Add(Prefab.Gem,GD.Load("res://Prefabs/Nodes/Gem.tscn") as PackedScene);
		prefabs.Add(Prefab.FinalArtifact,GD.Load("res://Prefabs/Nodes/FinalArtifact.tscn") as PackedScene);
		
		prefabs.Add(Prefab.Particle,GD.Load("res://Prefabs/Nodes/GenericParticle.tscn") as PackedScene);

		prefabs.Add(Prefab.SoundInst,GD.Load("res://Temple/FX/SoundInst.tscn") as PackedScene);
		prefabs.Add(Prefab.SoundInst2D,GD.Load("res://Temple/FX/SoundInst2D.tscn") as PackedScene);
		prefabs.Add(Prefab.MusicInst,GD.Load("res://Temple/FX/MusicInst.tscn") as PackedScene);
	}

	private void PreloadUI(){
		ui = new Dictionary<UI, PackedScene>();
	}


	public T SpawnPrefab<T>(Prefab p,Vector2 pos,Node parent = null) where T : Node{
		PackedScene prefab;
		if(prefabs.TryGetValue(p,out prefab)){
			return Spawn<T>(prefab,pos,parent);
		}

		return null;
	}

	public T SpawnUI<T>(UI p,Node parent = null) where T : Node{
		PackedScene prefab;
		if(ui.TryGetValue(p,out prefab)){
			return SpawnNonspatial<T>(prefab,parent);
		}

		return null;
	}

	private T SpawnNonspatial<T>(PackedScene prefab,Node parent = null) where T : Node{
		Node n = prefab.Instance() as Node;
		if(parent==null){
			parent = node;
		}
		parent.AddChild(n);
		return n as T;
	}


	private T Spawn<T>(PackedScene prefab,Vector2 pos,Node parent = null) where T : Node{
		Node2D n = prefab.Instance() as Node2D;
		if(parent==null){
			parent = node;
		}
		parent.AddChild(n);
		n.GlobalPosition = pos;
		return n as T;
	}

	public enum Prefab{
		MinerNode,
		TravelNode,
		GeneratorNode,
		ForceFieldNode,
		Battery,
		Gem,
		FinalArtifact,
		Particle,
		SoundInst,
		SoundInst2D,
		MusicInst
	}

	public enum UI{
		Counter
		
	}


}

