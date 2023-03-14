/// <summary>
/// Contains timing for locking the Character’s movement and action during animation, and also timing for attack chaining windows for button presses.
/// </summary>

using UnityEngine;


public class CharacterTiming:MonoBehaviour
{
	[HideInInspector] public CharacterController characterController;

	/// <summary>
	/// Lock timing for all the Character attacks and actions.
	/// </summary>
	public float TimingLock(Character character, string action)
    {
		float timing = 0f;
		if (character == Character.Brute)
		{
			if (action == "attack1") timing = 1f;
			else if (action == "attack2") timing = 1.1f;
			else if (action == "attack3") timing = 1.4f;
			else if (action == "dash") timing = 1.4f;
			else if (action == "jumpattack") timing = 1.1f;
			else if (action == "lighthit1") timing = 1f;
			else if (action == "move1") timing = 1.7f;
			else if (action == "range1") timing = 2.3f;
			else if (action == "revive") timing = 1.6f;
			else if (action == "special1") timing = 2.1f;
		}
		else if (character == Character.Knight)
		{
			if (action == "attack1") timing = 0.6f;
			else if (action == "attack2") timing = 0.7f;
			else if (action == "attack3") timing = 0.9f;
			else if (action == "dash") timing = 1.1f;
			else if (action == "dash2") timing = 0.65f;
			else if (action == "lighthit1") timing = 0.75f;
			else if (action == "move1") timing = 1f;
			else if (action == "move2") timing = 1.1f;
			else if (action == "range1") timing = 1.3f;
			else if (action == "revive") timing = 1.7f;
			else if (action == "sheath") timing = 1f;
			else if (action == "special1") timing = 1.3f;
			else if (action == "special2") timing = 0.9f;
		}
		return timing;
    }

	/// <summary>
	/// Chain timing windows for the Character attack chain button presses.
	/// </summary>
	public float TimingChain(Character character, string action)
	{
		float timing = 0f;
		if (character == Character.Brute)
		{
			if (action == "attack1") timing = 0.4f;
			else if (action == "attack1end") timing = 0.9f;
			else if (action == "attack2") timing = 0.4f;
			else if (action == "attack2end") timing = 0.7f;
		}
		else if (character == Character.Knight)
		{
			if (action == "attack1") timing = 0.1f;
			else if (action == "attack1end") timing = 0.8f;
			else if (action == "attack2") timing = 0.3f;
			else if (action == "attack2end") timing = 0.9f;
		}
		return timing;
	}
}