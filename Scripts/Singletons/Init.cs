using Godot;
using ChromeWing.Temple;

public class Init : Node2D {

	public static Init Instance;

	public override void _EnterTree(){
		Instance = this;
		InitSingletons();
	}

	public override void _Process(float delta){
		Events.Instance.process?.Invoke(delta);
	}

	public void InitSingletons(){
		ResourcesManager.EnsureExists();
		MapGenerator.EnsureExists();
		GameManager.EnsureExists();

	}
}
