using Godot;
using System;

public class ArtifactUI : Label{

	public override void _Ready(){
		Events.Instance.artifactUpdated += OnArtifactUpdated;
		ResourcesManager.Instance.RefreshUI();
	}

	public override void _ExitTree(){
		Events.Instance.artifactUpdated -= OnArtifactUpdated;
	}

	public void OnArtifactUpdated(int artifact){
		this.Text = string.Format("{0}/{1} Artifacts",artifact,MapGenerator.FinalArtifactCount);
	}

}