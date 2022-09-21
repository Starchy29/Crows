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

public enum Aim {
    None,
    Forward,
    Up,
    Air,
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

    public int RequiredPosition { get; set; } // any by default
    public CharacterScript RequiredPartner { get; set; } // allies only
    public int Cost { get; set; }
    public bool CanUse() {
        if(Cost > Global.Inst.BattleManager.AbilityPoints) {
            return false;
        }

        if(RequiredPosition >= 0 && user.GetPosition() != RequiredPosition) {
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

    //private Animation activeAnimation;
    //private bool running;
    //public bool Running { get { return running; } }

    private CharacterScript user;
    public List<CharacterScript> Targets { get; set; }
    public TurnMove NextMove { get; set; }

    public TurnMove(String name, CharacterScript user, MoveEffect effect, List<int[]> targetType) {
        //running = false;
        this.name = name;
        this.user = user;
        this.moveEffect = effect;
        this.targetGroups = targetType;
        RequiredPosition = -1; // means any position
        Cost = 0;
    }

    // executes the move's animation, ending with the mechanical effect
    public void Run() {
        //running = true;
        Animation activeAnimation = user.gameObject.AddComponent<Animation>();
        activeAnimation.CopyFrom(AnimationBlueprint);
        activeAnimation.OnComplete = () => { 
            moveEffect(user, Targets);
            Global.Inst.BattleManager.CompleteMove();
        };
    }

    //public void Update() {
    //    activeAnimation.Update();
    //    if(activeAnimation.Complete) {
    //        running = false;
    //        moveEffect(user, Targets);
    //    }
    //}

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
