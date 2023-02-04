using Godot;
using System;
using System.Collections.Generic;
using ChromeWing.Temple;

public class MinerNode : Node2D, IClickable
{

	protected CollisionShape2D shape2D;
	protected CircleShape2D circle;

	protected CollisionShape2D rangeShape2D;
	protected CircleShape2D rangeCircle;

	protected Sprite rangeIndicator;

	public List<Payload> payloads = new List<Payload>();

	[Export]
	protected Color minerAttachColor;

	[Export]
	protected MinerNode parent;

	private bool initialized;

	protected float timer;

	[Export]
	private float secondsPerPayload = 2f;

	[Export]
	public int findableCount = 10;

	[Export]
	public PayloadType generatePayloadType;

	protected int sent = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shape2D = GetNode(new NodePath("./CollisionShape2D")) as CollisionShape2D;
		circle = shape2D.Shape as CircleShape2D;
		
		rangeShape2D = GetNode(new NodePath("./Range")) as CollisionShape2D;
		rangeCircle = rangeShape2D.Shape as CircleShape2D;

		rangeIndicator = GetNode(new NodePath("./RangeIndicator")) as Sprite;

		
	}

	public override void _ExitTree(){
		Events.Instance.nodeSpawned -= OnNodeSpawned;
	}

	public MinerNode Init(){
		if(initialized){return this;}
		Events.Instance.nodeSpawned?.Invoke(this);

		Events.Instance.nodeSpawned += OnNodeSpawned;
		initialized=true;
		return this;
	}

	public void Attach(MinerNode parent){
		this.parent = parent;
		Vector2 pos = this.GlobalPosition;
		GetParent().RemoveChild(this);
		parent.AddChild(this);
		this.GlobalPosition = pos;

		Fog.Instance.UpdateFog(this.GlobalPosition);

		SceneManager.Instance.SpawnPrefab<PlayParticle>(SceneManager.Prefab.Particle,this.GlobalPosition).SetColor(minerAttachColor).Play();
		AudioManager.Instance.PlaySound2D(SoundID.Attach);
	}

	public void AddPayload(PayloadType type,bool allowOneOnly=false){
		if(allowOneOnly){
			payloads.Clear();
		}
		payloads.Add(new Payload(type));
	}

	public void AddPayload(Payload p){
		payloads.Add(p);
	}

	public void CheckGeneratePayload(float delta){
		if(sent>=findableCount){return;}
		timer+=1f/secondsPerPayload*delta*ResourcesManager.Instance.speedValue;
		if(timer>=1){
			var excessEnergy = Mathf.FloorToInt(timer);
			AddPayload(generatePayloadType,true);
			if(this.parent!=null){
				sent++;
			}
			timer -= excessEnergy;
		}
	}

	private void UpdatePayloads(float delta){
		for(int i=payloads.Count-1;i>=0;i--){
			if(parent==null){
				this.OnPayloadReachedEnd(payloads[i]);
				payloads.RemoveAt(i);
				continue;
			}
			if(payloads[i].Update(delta)){
				parent.AddPayload(payloads[i]);
				payloads.RemoveAt(i);
			}
		}
	}

	public override void _Process(float delta){
		UpdatePayloads(delta);
		Update();
	}

	public override void _Draw()
	{
		if(parent==null){return;}
		DrawLine(-Position,Vector2.Zero,Color.FromHsv(0,.5f,.2f,.8f),10,true);
		foreach(var p in payloads){
			DrawCircle(-Position.LinearInterpolate(Vector2.Zero,1f-p.percentToNext),p.GetSize(),p.GetColor());
		}
	}

	public virtual void OnPayloadReachedEnd(Payload payload){

	}

	public virtual bool IsTargetable(){
		return true;
	}
	public bool InClickRange(Vector2 pos){
		return (pos - this.GlobalPosition).Length()<circle.Radius;
	}
	public float GetRange(Vector2 pos){
		//GD.Print(this.Name+" global position thinks its at "+this.GlobalPosition);
		return (pos - this.GlobalPosition).Length();
	}

	public bool InHarvestableRange(Vector2 pos){
		//GD.Print("I am "+this.Name+", with rangeCircleRadius="+(rangeCircle.Radius*2)+", length = "+(pos - this.GlobalPosition).Length());
		return (pos - this.GlobalPosition).Length()<GetHarvestableRange();
	}

	public float GetHarvestableRange(){
		return rangeCircle.Radius;
	}

	public void OnNodeSpawned(MinerNode node){
		if(!(this is IFindable)){return;}
		var closest = SpatialManager.Instance.FindClosestNonFindable(this.GlobalPosition,this).closest;
		if(closest==node){
			this.Attach(node);
		}
	}

	public void SetRangeIndicator(bool enabled){
		rangeIndicator.Visible = enabled;
	}


}


public class Payload{
	public PayloadType type;
	public float percentToNext;
	public Payload(PayloadType type){
		this.type = type;
	}
	public bool Update(float delta){
		delta = delta*ResourcesManager.Instance.speedValue;
		switch(type){
			case PayloadType.Energy:
			percentToNext+=delta*2f;
			break;
			case PayloadType.Money:
			percentToNext+=delta*1.1f;
			break;
			case PayloadType.FinalArtifact:
			percentToNext+=delta*.6f;
			break;
		}
		if(percentToNext>=1){
			percentToNext--;
			return true;
		}else{
			return false;
		}
	}
	
	public float GetSize(){
		switch(type){
			case PayloadType.Energy:
			return 20*(1f+ResourcesManager.Instance.energyValue/5f);
			case PayloadType.Money:
			return 30;
			case PayloadType.FinalArtifact:
			return 100;
		}
		return 20;
	}
	public Color GetColor(){
		switch(type){
			case PayloadType.Energy:
			return new Color(0,0,.7f,1);
			case PayloadType.Money:
			return new Color(.9f,.3f,.7f,1);
			case PayloadType.FinalArtifact:
			return new Color(0,0,.5f,1);
		}
		return Color.FromHsv(0,0,0,1);
	}
}

public enum PayloadType{
	Energy,
	Money,
	FinalArtifact
}