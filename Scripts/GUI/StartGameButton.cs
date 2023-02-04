using Godot;
using System;

public class StartGameButton : Button{

	[Export]
	private NodePath titleScreenPath;
	[Export]
	private NodePath hudPath;

	private Control titleScreen,hud;

	public override void _Ready(){
		hud = GetNode<Control>(hudPath);
		titleScreen = GetNode<Control>(titleScreenPath);
		this.Connect("pressed",this,nameof(this.OnClicked));
	}

	public void OnClicked(){
		titleScreen.Visible = false;
		hud.Visible = true;
		GameManager.Instance.StartNewGame();
	}


}

/*!TITLE
You are a mining facility set out to recover the lost artifacts of the world.  Right click and drag to pan the camera while playing, and left click to place a mining node in range of the nearest node you have currently built to expand your network into the depths.  Build near gems and batteries to earn money and energy.  Money is used to buy upgrades.  Energy is used to construct additional nodes.  Explore the map to find all the missing artifacts.  Good luck!


*/