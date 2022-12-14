using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePriority {
    EnemySwap,
    Priority,
    Buff,
    Attack,
    Debuff
}

// can be multiple at once
public enum Aim : uint {
    None = 0,
    Forward = 1,
    Up = 2,
}

// defines how a move works and also tracks its animation progress
public class TurnMove
{
    // move definition
    public Animation AnimationBlueprint { get; set; }
    public delegate void MoveEffect(CharacterScript user, List<CharacterScript> targets);
    private MoveEffect moveEffect;

    private List<int[]> targetGroups; // each element is a group of enemies/allies that are targeted, 0-3 ground 4-7 air front to back
    public List<int[]> TargetGroups { get { return targetGroups; } }

    public MoveEffect SwapFunction; // if not null, this move is a character swap

    public List<int> RequiredPositions { get; set; } // any by default, lists each spot that is valid
    public CharacterScript RequiredPartner { get; set; } // allies only
    public int Cost { get; set; }
    public bool CanUse() {
        if(SwapFunction != null && Global.Inst.BattleManager.HasSwapped) {
            // can't swap more than once per turn
            return false;
        }

        if(Cost > Global.Inst.BattleManager.AbilityPoints) {
            return false;
        }

        if(RequiredPositions != null && !RequiredPositions.Contains(user.GetPosition())) {
            return false;
        }

        if(RequiredPartner != null) {
            // allies only
            int[] adjacents = new int[2];
            int pos = user.GetPosition();
            adjacents[0] = pos + 1;
            adjacents[0] = pos - 1;

            bool foundPartner = false;
            foreach(int index in adjacents) {
                if(index < 0 || index > 3) {
                    continue;
                }

                if(Global.Inst.BattleManager.Players[index] == RequiredPartner) {
                    foundPartner = true;
                }
            }
            if(!foundPartner) {
                return false;
            }
        }

        return true;
    }

    private String name;
    public String Name { get { return name; } }

    // execution process tracking
    private CharacterScript user;
    public List<CharacterScript> Targets { get; set; }
    public TurnMove NextMove { get; set; }
    public GameObject Display { get; set; } // Shows what the move will do

    public TurnMove(String name, CharacterScript user, MoveEffect effect, List<int[]> targetType) {
        //running = false;
        this.name = name;
        this.user = user;
        this.moveEffect = effect;
        this.targetGroups = targetType;
        RequiredPositions = null; // means any position
        Cost = 0;
    }

    // executes the move's animation, ending with the mechanical effect
    public void Run() {
        Animation activeAnimation = user.gameObject.AddComponent<Animation>();
        activeAnimation.CopyFrom(AnimationBlueprint);
        activeAnimation.OnComplete = () => { 
            moveEffect(user, Targets);
            Global.Inst.BattleManager.CompleteMove();
        };
    }

    // for swaps, executes the swap
    public void ExecuteSwap() {
        SwapFunction(user, Targets);
    }

    // common target types
    public static readonly List<int[]> AnyTarget = new List<int[]>() { 
        new int[]{ 0 }, new int[]{ 1 }, new int[]{ 2 }, new int[]{ 3 },
        new int[]{ 4 }, new int[]{ 5 }, new int[]{ 6 }, new int[]{ 7 },
    };

    public static readonly List<int[]> GroundTarget = new List<int[]>() {
        new int[]{ 0 }, new int[]{ 1 }, new int[]{ 2 }, new int[]{ 3 },
    };

    public static readonly List<int[]> FrontTarget = new List<int[]>() { new int[]{ 0 } };
}
