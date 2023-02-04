using Godot;
using System;
using ChromeWing.Temple;

public class BaseNode : MinerNode {

	

	public override void _Ready(){
		base._Ready();
		Init();
		Events.Instance.fogReset += OnFogReset;
		OnFogReset();
	}

	public void OnFogReset(){
		if(Fog.Instance==null){return;}
		Fog.Instance.UpdateFog(this.GlobalPosition);
		Fog.Instance.UpdateFog(this.GlobalPosition+new Vector2(-50,50));
		Fog.Instance.UpdateFog(this.GlobalPosition+new Vector2(50,50));
	}

	public override void _Process(float delta){
		base._Process(delta);
	}

	public override void OnPayloadReachedEnd(Payload payload){
		switch(payload.type){
			case PayloadType.Energy:
			ResourcesManager.Instance.AddEnergy(ResourcesManager.Instance.energyValue);
			break;
			case PayloadType.Money:
			ResourcesManager.Instance.AddMoney(1);
			break;
			case PayloadType.FinalArtifact:
			ResourcesManager.Instance.AddArtifact(1);
			AudioManager.Instance.PlaySound2D(SoundID.ArtifactArrive);
			break;
		}

		AudioManager.Instance.PlaySound2D(SoundID.PayloadArrive);
	}

}
