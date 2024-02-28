using Godot;

namespace ProjectCleanSword.Scripts.Player;

public partial class BladeAnimator : AnimationPlayer
{
	public void PlayAttack()
	{
		Play("attacking");
	}
}