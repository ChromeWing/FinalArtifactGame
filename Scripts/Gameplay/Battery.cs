using Godot;
using System;

public class Battery : MinerNode, IFindable {

	public override void _Ready(){
		base._Ready();
		Init();
	}

	public override void _Process(float delta){
		base._Process(delta);

		CheckGeneratePayload(delta);
	}

	public override bool IsTargetable()
	{
		return false;
	}

}
