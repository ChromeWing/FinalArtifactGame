using ChromeWing.Temple;
using Godot;
using System;


public class WonGameScreen : Control {

	[Export]
	private NodePath finalTimePath;

	private FinalTimeUI finalTimeLabel;
	public override void _Ready(){
		finalTimeLabel = GetNode(finalTimePath) as FinalTimeUI;
		Events.Instance.wonGame += OnWonGame;
		Events.Instance.startedGame += OnStartedGame;
	}

	public override void _ExitTree(){
		Events.Instance.wonGame -= OnWonGame;
		Events.Instance.startedGame -= OnStartedGame;
	}

	public void OnWonGame(int time){
		finalTimeLabel.SetFinalTime(time);
		this.Visible = true;
		
		AudioManager.Instance.PlaySound2D(SoundID.Win);
	}

	public void OnStartedGame(){
		this.Visible = false;
	}

}