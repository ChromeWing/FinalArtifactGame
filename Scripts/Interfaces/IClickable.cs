using Godot;

public interface IClickable{
	bool InClickRange(Vector2 cursor);
	float GetRange(Vector2 cursor);
	bool IsTargetable();
}