using Godot;
using System;

public class PlayParticle : Particles2D{

	private bool initialized = false;

	public override void _Ready(){
	}

	public override void _Process(float delta){
		if(!this.Emitting && initialized){
			GameManager.Destroy(this);
		}
	}

	public PlayParticle SetColor(Color color){
		ProcessMaterial.Set("color",color);
		return this;
	}

	public PlayParticle Play(){
		this.Emitting=true;
		this.initialized = true;
		return this;
	}


}
