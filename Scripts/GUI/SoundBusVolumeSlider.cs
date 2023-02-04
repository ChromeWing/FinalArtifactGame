using Godot;
using System;
using ChromeWing.Temple;

public class SoundBusVolumeSlider : HSlider {

	[Export]
	private string bus = "Master";

	private int busIndex = 0;

	public override void _Ready(){
		busIndex = AudioServer.GetBusIndex(bus);
		this.Connect("value_changed",this,nameof(this.OnSliderValueChanged));
		OnSliderValueChanged((float)this.Value);
	}

	public void OnSliderValueChanged(float value){
		AudioServer.SetBusVolumeDb(busIndex,value);
		if(value<=-30){
			AudioServer.SetBusMute(busIndex,true);
		}else{
			AudioServer.SetBusMute(busIndex,false);
		}
	}

}