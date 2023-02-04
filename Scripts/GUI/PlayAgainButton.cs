using Godot;
using System;

public class PlayAgainButton : Button {

	public override void _Ready(){
		this.Connect("pressed",this,nameof(this.OnClicked));
	}

	public void OnClicked(){
		GameManager.Instance.StartNewGame();
	}

}