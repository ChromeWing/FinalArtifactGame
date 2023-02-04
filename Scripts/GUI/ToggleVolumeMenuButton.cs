using Godot;
using System;

public class ToggleVolumeMenuButton : TextureButton {

	[Export]
	private NodePath volumeSlidersPath;
	private Control volumeSliders;

	public override void _Ready(){
		volumeSliders = GetNode<Control>(volumeSlidersPath);
		this.Connect("pressed",this,nameof(this.OnClicked));

		Events.Instance.clickedOffToolbar = OnClickedOffToolbar;
	}

	public void OnClickedOffToolbar(){
		volumeSliders.Visible = false;
	}

	public void OnClicked(){
		volumeSliders.Visible = !volumeSliders.Visible;
	}

}