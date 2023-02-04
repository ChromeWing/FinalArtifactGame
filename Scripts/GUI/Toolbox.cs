using Godot;
using System;
using System.Collections.Generic;

public class Toolbox : Node {

	public static void ClearSelection(){
		Instance.ClearSelectedType();
	}

	public static MinerNodeType GetSelectedType(){
		return Instance.selectedType;
	}

	public static Toolbox Instance;

	[Export]
	public ButtonGroup group;

	[Export]
	public MinerNodeType selectedType = MinerNodeType.None;

	private List<ToolboxButton> buttons;


	public override void _Ready(){
		Instance = this;
		buttons = new List<ToolboxButton>();
		foreach(var button in group.GetButtons()){
			var b = button as ToolboxButton;
			buttons.Add(b);
			b.Connect("pressed",this,nameof(this.OnButtonPressed));
		}
	}

	public void OnButtonPressed(){
		ToolboxButton b = group.GetPressedButton() as ToolboxButton;
		selectedType =  b.type;
		//GD.Print("pressed Button with type "+selectedType);
	}

	public void ClearSelectedType(){
		selectedType = MinerNodeType.None;
		foreach(var b in buttons){
			b.SetPressedNoSignal(false);
		}
	}

	public void SetEnabled(bool enabled){
		foreach(var b in buttons){
			b.Disabled = !enabled;
		}
	}

}