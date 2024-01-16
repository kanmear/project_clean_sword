namespace ProjectCleanSword.Scripts;

using Godot;

public static class GlobalConstants
{
	//NOTE Get the gravity from the project settings to be synced with RigidBody nodes.
	public static readonly float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
}