using Godot;
using System;
using ChromeWing.Temple;
public class Events : Singleton<Events>{

	public Action<int> energyUpdated;
	public Action<int> moneyUpdated;
	public Action<int> artifactUpdated;

	public Action<int> energyLevelUpdated;
	public Action<int> speedLevelUpdated;

	public Action<int> deployCostChanged;

	public Action<float> process;

	public Action<MinerNode> nodeSpawned;

	public Action clickedOffToolbar;

	public Action startedGame;
	public Action<int> wonGame;

	public Action fogReset;


}