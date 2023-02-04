using ChromeWing.Temple;
using Godot;
using System;

public class CameraPanning : Camera2D{

	public static CameraPanning Camera;

	public Action<InputEventMouseMotion> mouseMoveEvent;
	public Action<InputEventMouseButton> mouseButtonEvent;

	public override void _Ready(){
		Camera = this;
		ControlsManager.EnsureExists();

		
		AudioManager.Instance.PlayMusic(MusicID.Mystic);
	}


	public override void _Input(InputEvent @event){
		if(@event is InputEventMouseMotion){
			OnMouseMove(@event as InputEventMouseMotion);
		}
		if(@event is InputEventMouseButton){
			OnMouseButton(@event as InputEventMouseButton);
		}
	}

	private void OnMouseMove(InputEventMouseMotion m){
		mouseMoveEvent?.Invoke(m);
	}

	private void OnMouseButton(InputEventMouseButton m){
		mouseButtonEvent?.Invoke(m);
	}

}