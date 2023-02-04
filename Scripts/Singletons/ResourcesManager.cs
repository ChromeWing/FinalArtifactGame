using Godot;
using System;
using ChromeWing.Temple;
public class ResourcesManager : Singleton<ResourcesManager>{


	public int energy{get;private set;}
	public int money{get;private set;}
	public int artifact{get;private set;}

	public int speedLevel{get;private set;}
	public int energyLevel{get;private set;}

	public int energyValue{get{
		return energyLevelValues[energyLevel-1];
	}}

	public float speedValue{get{
		return speedLevelValues[speedLevel-1];
	}}

	private float gameTimer;

	private float energyPerSecond {get{
		return 0;
	}}

	private float energyTimer;

	private int[] energyLevelPrices = new int[10]{0,15,20,30,40,50,50,50,50,50};
	private int[] speedLevelPrices = new int[5]{0,10,20,40,60}; //0,5,8,12,16
	
	private int[] energyLevelValues = new int[10]{1,2,3,4,5,6,7,8,9,10};
	private float[] speedLevelValues = new float[5]{1f,2f,3f,4f,7f};

	public int GetPriceOfEnergy(){
		if(IsCappedEnergyLevel()){return 0;}
		return energyLevelPrices[energyLevel];
	}

	public int GetPriceOfSpeed(){
		if(IsCappedSpeedLevel()){return 0;}
		return speedLevelPrices[speedLevel];
	}

	public bool IsCappedEnergyLevel(){
		return energyLevel-1>=energyLevelPrices.Length-1;
	}
	public bool IsCappedSpeedLevel(){
		return speedLevel-1>=speedLevelPrices.Length-1;
	}


	public ResourcesManager(){

		Events.Instance.process += OnProcess;
		Events.Instance.nodeSpawned += OnNodeSpawned;

		Events.Instance.artifactUpdated += CheckWin;
	}

	public void CheckWin(int artifact){
		if(this.artifact==MapGenerator.FinalArtifactCount){
			Events.Instance.wonGame?.Invoke(Mathf.RoundToInt(gameTimer));
		}
	}

	public void Reset(){
		this.gameTimer = 0;
		this.SetEnergy(300);
		this.SetArtifact(0);
		this.SetMoney(0);
		this.SetEnergyLevel(1);
		this.SetSpeedLevel(1);
	}


	public void RefreshUI(){
		Events.Instance.energyUpdated?.Invoke(this.energy);
		Events.Instance.moneyUpdated?.Invoke(this.money);
		Events.Instance.energyLevelUpdated?.Invoke(this.energyLevel);
		Events.Instance.speedLevelUpdated?.Invoke(this.speedLevel);
		Events.Instance.artifactUpdated?.Invoke(this.artifact);
	}

	public void OnProcess(float delta){
		gameTimer+=delta;
		energyTimer+=energyPerSecond*delta;
		if(energyTimer>=1){
			var excessEnergy = Mathf.FloorToInt(energyTimer);
			AddEnergy(excessEnergy);
			energyTimer -= excessEnergy;
		}
	}

	public void OnNodeSpawned(MinerNode node){
		if(node is GeneratorNode){
			
		}
	}

	public void SetEnergy(int energy){
		this.energy = energy;
		Events.Instance.energyUpdated?.Invoke(this.energy);
	}

	public void SetMoney(int money){
		this.money = money;
		Events.Instance.moneyUpdated?.Invoke(this.money);
	}


	public void AddEnergy(int energy=1){
		this.energy+=energy;
		Events.Instance.energyUpdated?.Invoke(this.energy);
	}

	public void AddMoney(int money=1){
		this.money+=money;
		Events.Instance.moneyUpdated?.Invoke(this.money);
	}

	public void AddArtifact(int artifact=1){
		this.artifact+=artifact;
		Events.Instance.artifactUpdated?.Invoke(this.artifact);
	}

	public void SetArtifact(int artifact){
		this.artifact = artifact;
		Events.Instance.artifactUpdated?.Invoke(this.artifact);
	}

	public void SetEnergyLevel(int energy){
		this.energyLevel = energy;
		Events.Instance.energyLevelUpdated?.Invoke(this.energyLevel);
	}

	public void SetSpeedLevel(int speed){
		this.speedLevel = speed;
		Events.Instance.speedLevelUpdated?.Invoke(this.speedLevel);
	}

	public bool AttemptBuyEnergyLevel(){
		if(IsCappedEnergyLevel()){
			return false;
		}
		bool bought = false;
		SpendMoney(GetPriceOfEnergy(),()=>{
			AddEnergyLevel();
			bought = true;
		});

		return bought;
	}

	public bool AttemptBuySpeedLevel(){
		if(IsCappedSpeedLevel()){
			return false;
		}
		bool bought = false;
		SpendMoney(GetPriceOfSpeed(),()=>{
			AddSpeedLevel();
			bought = true;
		});

		return bought;
	}

	public void AddEnergyLevel(int energy=1){
		this.energyLevel+=energy;
		Events.Instance.energyLevelUpdated?.Invoke(this.energyLevel);
	}

	public void AddSpeedLevel(int speed=1){
		this.speedLevel+=speed;
		Events.Instance.speedLevelUpdated?.Invoke(this.speedLevel);
	}

	public void SpendEnergy(int energy,Action onSuccess){
		if(this.energy>=energy){
			this.energy-=energy;
			Events.Instance.energyUpdated?.Invoke(this.energy);
			onSuccess();
		}
	}
	public void SpendMoney(int money,Action onSuccess){
		if(this.money>=money){
			this.money-=money;
			Events.Instance.moneyUpdated?.Invoke(this.money);
			onSuccess();
		}
	}

	public void SpendEnergyAndMoney(int energy,int money,Action onSuccess){
		if(this.energy>=energy && this.money>=money){
			this.energy-=energy;
			this.money-=money;
			Events.Instance.energyUpdated?.Invoke(this.energy);
			Events.Instance.moneyUpdated?.Invoke(this.money);
			onSuccess();
		}
	}



}
