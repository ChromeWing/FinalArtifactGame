using Godot;
using System;

public class FinalTimeUI : Label {

	public void SetFinalTime(int time){
		TimeSpan t = TimeSpan.FromSeconds(time);
		Text = string.Format("{0:D2}:{1:D2}:{2:D2}",t.Hours,t.Minutes,t.Seconds);
	}

}