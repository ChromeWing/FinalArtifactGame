using Godot;
using System;
using System.Collections.Generic;

namespace ChromeWing.Temple{
	public class AudioManager : Singleton<AudioManager>{

		private int masterBus = AudioServer.GetBusIndex("Master");
		private int sfxBus = AudioServer.GetBusIndex("Sfx");
		private int musicBus = AudioServer.GetBusIndex("Music");

		public List<SoundInst2D> playingInsts2D = new List<SoundInst2D>();
		public List<SoundInst> playingInsts = new List<SoundInst>();

		public SoundInst2D playingMusic;
		Dictionary<SoundID,AudioStream> streams = new Dictionary<SoundID, AudioStream>();
		Dictionary<MusicID,AudioStream> musicStreams = new Dictionary<MusicID, AudioStream>();

		public AudioManager(){
			streams.Add(SoundID.Attach,ResourceLoader.Load("res://Audio/sfx/ionhit.wav") as AudioStream);
			streams.Add(SoundID.PayloadArrive,ResourceLoader.Load("res://Audio/sfx/menu_taping.wav") as AudioStream);
			streams.Add(SoundID.ArtifactArrive,ResourceLoader.Load("res://Audio/sfx/menu_new.wav") as AudioStream);
			streams.Add(SoundID.Win,ResourceLoader.Load("res://Audio/sfx/kinetichit.wav") as AudioStream);


			
			musicStreams.Add(MusicID.Mystic,ResourceLoader.Load("res://Audio/MysticalTheme.mp3") as AudioStream);

			Events.Instance.process += OnProcess;
		}

		public void OnProcess(float delta){
			playingInsts.Clear();
			playingInsts2D.Clear();
		}

		public void PlaySound2D(SoundID sound){
			Node parent = CameraPanning.Camera;
			Vector2 pos = CameraPanning.Camera.GlobalPosition;
			AudioStream s = streams[sound];
			SoundInst2D inst = SceneManager.Instance.SpawnPrefab<SoundInst2D>(SceneManager.Prefab.SoundInst2D,pos,parent);
			playingInsts2D.Add(inst.PlaySound(s));
		}

		public void PlaySound3D(SoundID sound){
			Node parent = CameraPanning.Camera;
			Vector2 pos = CameraPanning.Camera.GlobalPosition;
			AudioStream s = streams[sound];
			SoundInst inst = SceneManager.Instance.SpawnPrefab<SoundInst>(SceneManager.Prefab.SoundInst,pos,parent);
			playingInsts.Add(inst.PlaySound(s));
		}

		public void PlayMusic(MusicID music){
			Node parent = CameraPanning.Camera;
			Vector2 pos = CameraPanning.Camera.GlobalPosition;
			AudioStream s = musicStreams[music];
			SoundInst2D inst = SceneManager.Instance.SpawnPrefab<SoundInst2D>(SceneManager.Prefab.MusicInst,pos,parent);
			if(GameManager.IsValid(playingMusic)){
				playingMusic.Stop();
			}
			playingMusic = inst.PlaySound(s);
		}



	}

	public enum SoundID{
		Attach,
		PayloadArrive,
		ArtifactArrive,
		Win
	}
	public enum MusicID{
		Mystic
	}
}