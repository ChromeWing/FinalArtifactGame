using Godot;
using System;

public class ToolboxButton : TextureButton {

	[Export]
	public MinerNodeType type;

}

public enum MinerNodeType{
	None,
	Travel,
	Mining,
	Generator,
	ForceField
}