#nullable enable
using System;
using System.Collections.Generic;
using Differ.Data;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Actions.Lookups;
using Glaidiator.Model.Collision;
using Glaidiator.Model.Resources;
using Glaidiator.Model.Utils;
using RPGCharacterAnims.Actions;
using UnityEngine;
using Attack = Glaidiator.Model.Actions.Attack;
using BoxCollider = Glaidiator.Model.Collision.BoxCollider;
using Collider2D = Glaidiator.Model.Collision.Collider2D;

namespace Glaidiator.Model
{
    public class Character : StateMachine
    {
	    private enum CharacterState
	    {
		    Dead = 0,
		    Idling = 1,
		    Moving = 2,
		    Attacking = 3,
		    Blocking = 4,
		    Dodging = 5
	    }

	    #region Flags
	    
	    public bool CanMove = true;
	    public bool CanAction = true;

	    #endregion

	    #region Stats

	    public bool IsDead = false;
	    public float DamageTaken = 0f;

	    #endregion
	    
	    #region Attributes

	    private Enum _newState;

	    public World World { get; private set; } = null!;
	    public readonly Movement Movement;
	    public readonly Health Health;
	    public readonly Stamina Stamina;
	    public readonly CharacterHitbox Hitbox;
	    
	    private Input _inputs;
	    
	    public readonly Dictionary<string, IAction> Actions = new Dictionary<string, IAction>();
	    public readonly List<ICooldown> Cooldowns  = new List<ICooldown>();
	    public IAction? ActiveAction { get; private set; }
	    #endregion

	    #region Events

	    public Action? onDeath;
	    public Action? onLowStamina;
	    public Action? onMoveTick;
	    public Action? onMoveEnd;
	    public Action? onAttackStart;
	    public Action? onAttackEnd;
	    public Action? onBlockStart;
	    public Action? onBlockEnd;
	    public Action? onDodgeStart;
	    public Action? onDodgeEnd;

	    private void OnDeath() => onDeath?.Invoke();
	    private void OnLowStamina() => onLowStamina?.Invoke();
	    private void OnMoveTick() => onMoveTick?.Invoke();
	    private void OnMoveEnd() => onMoveEnd?.Invoke();
	    private void OnAttackStart() => onAttackStart?.Invoke();
	    private void OnAttackEnd() => onAttackEnd?.Invoke();
	    private void OnBlockStart() => onBlockStart?.Invoke();
	    private void OnBlockEnd() => onBlockEnd?.Invoke();
	    private void OnDodgeStart() => onDodgeStart?.Invoke();
	    private void OnDodgeEnd() => onDodgeEnd?.Invoke();

	    #endregion

	    #region Initialization

	    public Character(Vector3 position, Quaternion rotation)
	    {
		    _newState = state.current;
		    Movement = new Movement(position, rotation);
		    Hitbox = new CharacterHitbox(new CircleCollider(Movement.Position.xz(), 0.75f,Vector2.zero,  false), this);
		    Health = new Health(100.0f, 0.01f);
		    Stamina = new Stamina(100.0f, 0.05f);
		    CurrentState = CharacterState.Idling;
		    IsDead = false; 

		    // init actions
		    Actions.Add("atkLight", 
			    new Attack(
					new ActionInfo((int)ActionLookup.AttackLight, "Light Attack", 10f, false, false, 0.9f), 
					new Hitbox<Attack>(
						new BoxCollider(Vector2.zero, new Vector2(2, 2), new Vector2(0, 1), true), 
						this,
						0.6f),
					10f, 1.2f, 0.2f));
		    Actions.Add("atkHeavy", 
			    new Attack(
				    new ActionInfo((int)ActionLookup.AttackHeavy, "Heavy Attack",20f, false, false, 1.8f), 
				    new Hitbox<Attack>(
					    new BoxCollider(Vector2.zero, new Vector2(3,3),new Vector2(0, 1.5f), true),
					    this,
					    1.2f),
				    25f, 3.3f, 0.4f));
		    Actions.Add("atkRanged",
			    new AttackRanged(
				    new ActionInfo((int)ActionLookup.AttackRanged, "Ranged Attack",15f, false, false, 1.5f), 
				    new ProjectileHitbox(
					    new CircleCollider(Vector2.zero, 1.0f,Vector2.zero, true),
					    this,
					    15f, 
					    20f
				    ), 
				    10f, 5.5f));
		    Actions.Add("block", new Block(new ActionInfo((int)ActionLookup.Block, "Block",10f, false, false,1.0f), 3.0f));
		    Actions.Add("dodge", new Dodge(new ActionInfo((int)ActionLookup.Dodge, "Dodge",25f,false, false,0.5f), 0.8f));
	    }
	    
	    #endregion

	    #region Checks

	    private bool HasEnoughStamina(IAction action)
	    {
		    return action.Action.Cost <= Stamina.Current;
	    }

	    private bool IsOnCooldown(string name)
	    {
		    return Cooldowns.Exists(c => c.Name == name);
	    }

	    #endregion

	    #region Setters

	    public void SetWorld(World world)
	    {
		    World = world;
		    Hitbox.Register();
	    }

	    public void SetInputs(Input inputs)
	    {
		    _inputs = inputs;
		    //Debug.Log(JsonUtility.ToJson(inputs, true));
	    }

	    private void SetCanFlags(bool movement, bool action)
	    {
		    CanMove = movement;
		    CanAction = action;
	    }

	    private void SetActiveAction(IAction action)
	    {
		    Stamina.Subtract(action.Action.Cost);
		    ActiveAction = action;
		    SetCanFlags(ActiveAction.Action.CanMove, ActiveAction.Action.CanAction);
		    ActiveAction.Start();
		    if(ActiveAction is ICooldown cd) Cooldowns.Add(cd.SetOnCooldown());
	    }
	    
	    private void ResetActiveAction()
	    {
		    ActiveAction?.End();
		    ActiveAction = null;
		    SetCanFlags(true, true);
	    }

	    #endregion

	    #region Update

	    public override void Tick(float deltaTime)
	    {
		    // general system ticks
		    UpdateCooldowns(deltaTime);
		    // init new state 
		    _newState = CurrentState;
		    
		    if (IsDead) _newState = CharacterState.Dead;
		    else
		    {
			    UpdateActiveAction(deltaTime);
			    Health.Regen(deltaTime);
			    Stamina.Regen(deltaTime);
			    Idling();// if not doing anything and not moving
			    // if can move
			    if (CanMove) Moving();

			    // if can action
			    if(CanAction)
			    {
				    if (_inputs.attackLight || _inputs.attackHeavy || _inputs.attackRanged)
					    Attacking();
				    else if (_inputs.block)
					    Blocking();
				    else if (_inputs.dodge)
					    Dodging();
			    }
		    }
		    
		    // switch state
		    CurrentState = _newState;
		    state.Tick(deltaTime);
	    }
		/**
		 * make cooldowns tick!
		 */
	    private void UpdateCooldowns(float deltaTime)
	    {
		    for (int index = Cooldowns.Count - 1; index >= 0; index--)
		    {
			    ICooldown cd = Cooldowns[index];
			    if (!cd.Cooldown.Tick(deltaTime))
				    Cooldowns.RemoveAt(index);
		    }
	    }

		private void UpdateActiveAction(float deltaTime)
		{
			if (ActiveAction is null || ActiveAction.Tick(deltaTime)) return;
			CurrentState = CharacterState.Idling;
			ResetActiveAction();
		}

		private void UpdateFacingDirection()
		{
			if (_inputs.facing == Vector3.zero) return;
			Movement.Face(_inputs.facing);
		}

	    #endregion
	    
        #region States

        #region Dead
        private void Dead_Enter()
        {
	        if (ActiveAction is not null) { ActiveAction = null; }
	        Hitbox.Deregister();
	        Cooldowns.Clear();
	        Stamina.Subtract(Stamina.Current);
	        SetCanFlags(false, false);
	        OnDeath();
        }


        #endregion

        #region Idling
        
        private void Idling()
        {
	        if (ActiveAction is null) _newState = CharacterState.Idling;
        }

        private void Idling_Tick(float deltaTime)
        {
	        UpdateFacingDirection();
        }

        #endregion

        #region Moving
        private void Moving()
        {
	        if (_inputs.move != Vector3.zero) _newState = CharacterState.Moving;
        }

        private void Moving_Enter()
        {
	       
        }
        
        private void Moving_Tick(float deltaTime)
        {
	        Movement.Move(_inputs.move, deltaTime);
	        OnMoveTick();
        }
        
        private void Moving_Exit()
        {
	        Movement.Stop();
	        OnMoveEnd();
        }

        #endregion

        #region Attacking
        
        private void Attacking()
        {
	        IAction? attack = null;
	        if (_inputs.attackLight)
	        {
		        attack = Actions["atkLight"];
	        }
	        else if (_inputs.attackHeavy)
	        {
		        attack = Actions["atkHeavy"];
	        }
	        else if (_inputs.attackRanged)
	        {
		        attack = Actions["atkRanged"];
	        }

	        if (attack is null || IsOnCooldown(attack.Action.Name))
	        {
		        return;
	        }
	        if (!HasEnoughStamina(attack))
	        {
		        OnLowStamina();
		        return;
	        }
	        SetActiveAction(attack);
	        _newState = CharacterState.Attacking;
        }
		
        private void Attacking_Enter()
        {
	        if (ActiveAction is not Attack atk) return;
	        UpdateFacingDirection();
	        OnAttackStart();
        }
        
        private void Attacking_Tick(float deltaTime)
        {
	        if (ActiveAction is not Attack atk) return;
	        if (atk.Delay.Tick(deltaTime)) return;
	        if (atk.ActiveHitbox == null) atk.SpawnHitbox(Movement.LastDir.xz());
        }
        
        private void Attacking_Exit()
        {
	        ResetActiveAction();
	        OnAttackEnd();
        }

        #endregion
        
        #region Blocking

        private void Blocking()
        {
	        IAction block = Actions["block"];
	        if (IsOnCooldown(block.Action.Name)) return;
	        if (!HasEnoughStamina(block))
	        {
		        OnLowStamina();
		        return;
	        }
	        SetActiveAction(block);
			_newState = CharacterState.Blocking;
        }
        private void Blocking_Enter()
        {
	        if (ActiveAction is null or not Block) return;
	        UpdateFacingDirection();
	        OnBlockStart();
        }
        
        private void Blocking_Tick(float deltaTime)
        {
        }
        
        private void Blocking_Exit()
        {
	        ResetActiveAction();
	        OnBlockEnd();
        }
        #endregion
        
        #region Dodging

        private void Dodging()
        {
	        IAction dodge = Actions["dodge"];
	        if (IsOnCooldown(dodge.Action.Name)) return;
	        if (!HasEnoughStamina(dodge))
	        {
		        OnLowStamina();
		        return;
	        }
	        SetActiveAction(dodge);
	        _newState = CharacterState.Dodging;
        }
        private void Dodging_Enter()
        {
	        if (ActiveAction is null or not Dodge) return;
	        UpdateFacingDirection();
	        (ActiveAction as Dodge)!.Direction = _inputs.move == Vector3.zero ? _inputs.move : _inputs.facing;
	        OnDodgeStart();
        }
        
        private void Dodging_Tick(float deltaTime)
        {
	        Movement.Dodge(Movement.LastDir, deltaTime);
        }
        
        private void Dodging_Exit()
        {
	        ResetActiveAction();
	        OnDodgeEnd();
        }
        #endregion

        #endregion

        #region Logic

        public void GetHit(Attack attack)
        {
	        switch ((CharacterState)CurrentState)
	        {
		        case CharacterState.Blocking:
			        Health.Add(10f);
			        return;
		        case CharacterState.Dodging:
			        return;
		        case CharacterState.Idling:
		        case CharacterState.Moving:
		        case CharacterState.Attacking:
		        default:
			        DamageTaken += attack.Damage;
			        Health.Subtract(attack.Damage);
			        if (Health.Current <= 0f)
			        {
				        IsDead = true;
			        }
			        return;
	        }
        }

        #endregion
    }
}