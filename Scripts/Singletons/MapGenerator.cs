using Godot;
using System;
using ChromeWing.Temple;
public class MapGenerator : Singleton<MapGenerator>{

	const int BatteryCount = 70;
	const int GemCount = 60;

	public const int FinalArtifactCount = 3;

	const float width = 6600;
	const float depth = 10000;

	private RandomNumberGenerator RNG = new RandomNumberGenerator();

	public MapGenerator(){
		//Generate();
	}


	public void Generate(){

		GenerateBatteries();
		GenerateGems();
		GenerateFinalArtifact();

	}


	private void GenerateBatteries(){
		GetRandomPositionsIntoDepths(BatteryCount,(p)=>{
			SceneManager.Instance.SpawnPrefab<Battery>(SceneManager.Prefab.Battery,p);
		});
	}

	private void GenerateGems(){
		GetRandomPositionsIntoDepths(GemCount,(p)=>{
			SceneManager.Instance.SpawnPrefab<Gem>(SceneManager.Prefab.Gem,p);
		});
	}

	private void GenerateFinalArtifact(){
		GetRandomPositionsIntoDepths(FinalArtifactCount,(p)=>{
			SceneManager.Instance.SpawnPrefab<FinalArtifact>(SceneManager.Prefab.FinalArtifact,p);
		});
	}


	private void GetRandomPositionsIntoDepths(int count,Action<Vector2> withPos){
		float distancePerPos = (depth-200)/count;
		for(float y = depth-100;y>100;y-=distancePerPos){
			float x = RNG.RandfRange(-width/2f+200,width/2f-200);
			withPos(new Vector2(x,y));
		}
	}
	

}