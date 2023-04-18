#nullable enable
using System;
using System.Collections.Generic;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Actions.Lookups;
using Glaidiator.Model.Collision;
using Glaidiator.Model.Resources;
using Glaidiator.Model.Utils;
using RPGCharacterAnims.Actions;
using UnityEngine;
using Attack = Glaidiator.Model.Actions.Attack;
using Collider2D = Glaidiator.Model.Collision.Collider2D;

namespace Glaidiator.Model
{
    public class Character : StateMachine
    {
	    private enum CharacterState
	    {
		    Idling = 0,
		    Moving = 1,
		    Attacking = 2,
		    Blocking = 3,
		    Dodging = 4
	    }

	    #region Flags
	    
	    public bool CanMove = true;
	    public bool CanAction = true;

	    #endregion
	    
	    #region Attributes

	    private Enum _newState;
	    
	    public readonly Movement Movement;
	    public readonly Health Health;
	    public readonly Stamina Stamina;
	    public readonly CharacterHitbox Hitbox;
	    
	    private Input _inputs;

	    private readonly Dictionary<string, IAction> _actions = new Dictionary<string, IAction>();
	    public readonly List<ICooldown> Cooldowns  = new List<ICooldown>();
	    public IAction? ActiveAction { get; private set; }
	    #endregion

	    #region Events
	    
	    public Action? onLowStamina;
	    public Action? onMoveTick;
	    public Action? onMoveEnd;
	    public Action? onAttackStart;
	    // public Action? onAttackEnd;
	    public Action? onBlockStart;
	    public Action? onBlockEnd;
	    public Action? onDodgeStart;
	    public Action? onDodgeTick;
	    public Action? onDodgeEnd;
	    
	    private void OnLowStamina() => onLowStamina?.Invoke();
	    private void OnMoveTick() => onMoveTick?.Invoke();
	    private void OnMoveEnd() => onMoveEnd?.Invoke();
	    private void OnAttackStart() => onAttackStart?.Invoke();
	    // private void OnAttackEnd() => onAttackEnd?.Invoke();
	    private void OnBlockStart() => onBlockStart?.Invoke();
	    private void OnBlockEnd() => onBlockEnd?.Invoke();
	    private void OnDodgeStart() => onDodgeStart?.Invoke();
	    private void OnDodgeTick() => onDodgeTick?.Invoke();
	    private void OnDodgeEnd() => onDodgeEnd?.Invoke();

	    #endregion

	    #region Initialization

	    public Character(Transform transform)
	    {
		    // TODO: insert actual logic
		    _newState = state.current;
		    Movement = new Movement(transform);
		    Hitbox = new CharacterHitbox(new Circle(Movement.Position.xz(), Vector2.zero, false, 0.75f), this);
		    Health = new Health(100.0f);
		    Stamina = new Stamina(100.0f, 0.05f);
		    CurrentState = CharacterState.Idling;

		    // init actions
		    _actions.Add("atkLight", 
			    new Attack(
					new ActionInfo((int)ActionLookup.AttackLight, "Light Attack", 10f, false, false, 0.9f), 
					new Hitbox<Attack>(
						new AABB(Vector2.zero, new Vector2(0, 1), true, Vector2.one), 
						this,
						0.6f),
					10f));
		    _actions.Add("atkHeavy", 
			    new Attack(
				    new ActionInfo((int)ActionLookup.AttackHeavy, "Heavy Attack",20f, false, false, 1.8f), 
				    new Hitbox<Attack>(
					    new AABB(Vector2.zero, new Vector2(0, 1), true, Vector2.one * 2f),
					    this,
					    1.2f),
				    25f, 3.3f));
		    _actions.Add("atkRanged",
			    new AttackRanged(
				    new ActionInfo((int)ActionLookup.AttackRanged, "Ranged Attack",15f, false, false, 1.5f), 
				    new ProjectileHitbox(
					    new Circle(Vector2.zero, new Vector2(0, 0.5f), true, 1.0f),
					    this,
					    15f, 
					    7.5f
				    ), 
				    10f, 5.5f));
		    _actions.Add("block", new Block(new ActionInfo((int)ActionLookup.Block, "Block",10f, false, false,1.0f), 3.0f));
		    _actions.Add("dodge", new Dodge(new ActionInfo((int)ActionLookup.Dodge, "Dodge",25f,false, false,0.5f), 1.0f));
	    }
	    
	    #endregion

	    #region Checks

	    private bool HasEnoughStamina(IAction action)
	    {
		    return action.Action.Cost <= Stamina.Current;
	    }

	    private bool IsOnCooldown<T>(object obj) where T : class, ICooldown
	    {
		    return Cooldowns.Contains((obj as T)!);
	    }

	    #endregion

	    #region Setters

	    public void SetInputs(Input inputs)
	    {
		    _inputs = inputs;
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
	    }
	    
	    private void ResetActiveAction()
	    {
		    ActiveAction = null;
		    SetCanFlags(true, true);
	    }

	    #endregion

	    #region Update

	    public override void Tick(float deltaTime)
	    {
		    // general system ticks
		    UpdateCooldowns(deltaTime);
		    UpdateActiveAction(deltaTime);
		    Stamina.Regen(deltaTime);

		    // init new state 
		    _newState = CurrentState;
		    
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
		    // switch state
		    CurrentState = _newState;
		    state.Tick(deltaTime);
	    }
		/**
		 * make cooldowns tick!
		 */
	    private void UpdateCooldowns(float deltaTime)
	    {
		    for (int index = 0; index < Cooldowns.Count; index++)
		    {
			    ICooldown cd = Cooldowns[index];
			    if (!cd.Cooldown.Tick(deltaTime))
				    Cooldowns.RemoveAt(index);
		    }
	    }

		private void UpdateActiveAction(float deltaTime)
		{
			if (ActiveAction is not null && !ActiveAction.Tick(deltaTime)) ActiveAction = null;
		}

	    #endregion
	    
        #region States

        #region Idling
        
        private void Idling()
        {
	        if (ActiveAction is null) _newState = CharacterState.Idling;
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
		        attack = _actions["atkLight"];
	        }
	        else if (_inputs.attackHeavy)
	        {
		        attack = _actions["atkHeavy"];
	        }
	        else if (_inputs.attackRanged)
	        {
		        attack = _actions["atkRanged"];
	        }
	        if (attack is null || IsOnCooldown<ICooldown>(attack)) return;
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
	        if (ActiveAction is not Attack attack) return;
	        attack.SpawnHitbox(Movement.LastDir.xz());
	        Cooldowns.Add(attack.SetOnCooldown());
	        OnAttackStart();
        }
        
        private void Attacking_Tick(float deltaTime)
        {
	       // do activeattack stuff
        }
        
        private void Attacking_Exit()
        {
	        ResetActiveAction();
        }

        #endregion
        
        #region Blocking

        private void Blocking()
        {
	        IAction block = _actions["block"];
	        if (IsOnCooldown<Block>(block)) return;
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
	        Cooldowns.Add((ActiveAction as Block)!.SetOnCooldown());
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
	        IAction dodge = _actions["dodge"];
	        if (IsOnCooldown<Dodge>(dodge)) return;
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
	        (ActiveAction as Dodge)!.Direction = _inputs.move == Vector3.zero ? _inputs.move : Movement.LastDir;
	        Cooldowns.Add((ActiveAction as Dodge)!.SetOnCooldown());
	        OnDodgeStart();
        }
        
        private void Dodging_Tick(float deltaTime)
        {
	        Movement.Dodge(Movement.LastDir, deltaTime);
	        OnDodgeTick();
        }
        
        private void Dodging_Exit()
        {
	        ResetActiveAction();
	        OnDodgeEnd();
        }
        #endregion

        #endregion
    }
}