using Godot;
using System;

namespace ChromeWing.Temple{

	public class SoundInst2D : AudioStreamPlayer2D{

		public SoundInst2D PlaySound(AudioStream stream){
			foreach(var inst in AudioManager.Instance.playingInsts2D){
				if(GameManager.IsValid(inst) && inst.Stream == stream){
					RemoveSelf();
					return this;
				}
			}

			this.Stream = stream;
			this.Connect("finished",this,nameof(this.RemoveSelf));
			Play();
			return this;
		}


		private void RemoveSelf(){
			var parent = this.GetParent();
			if(parent!=null){
				parent.RemoveChild(this);
			}
			this.QueueFree();
		}

	}

}
