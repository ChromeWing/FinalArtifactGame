using Godot;
using System;

namespace ChromeWing.Temple{

	public class SoundInst : AudioStreamPlayer{

		public SoundInst PlaySound(AudioStream stream){
			foreach(var inst in AudioManager.Instance.playingInsts){
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
