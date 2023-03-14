/// <summary>
/// Contains enum data for the Character’s type, state, and animation triggers.
/// </summary>
/// <summary>
/// The type of character.  Determines which animations can play, and the
/// timings for those animations in CharacterTiming.cs.
/// </summary>
public enum Character
{
	Brute,
	Knight
}

/// <summary>
/// The different movement / situational states the Character can be in.
/// </summary>
public enum CharacterState
{
	Idle = 0,
	Move = 1,
	Block = 2,
	Dodge = 3
}

/// <summary>
/// Enum to use with the "TriggerNumber" parameter of the animator. Convert to (int) to set.
/// </summary>
public enum AnimatorTrigger
{
	NoTrigger = 0,
	ActionTrigger = 1,
	DodgeTrigger = 2,
	AttackLightTrigger = 3,
	AttackHeavyTrigger = 5,
	AttackRangedTigger = 6,
	BlockTrigger = 7,
	BlockBreakTrigger = 8,
	LightHitTrigger = 9,
	DeathTrigger = 10
}