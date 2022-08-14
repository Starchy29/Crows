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
    private readonly Animation animationBlueprint;
    public delegate void MoveEffect(CharacterScript user, List<CharacterScript> targets);
    private MoveEffect moveEffect;

    private List<int[]> targetGroups; // each element is a group of enemies/allies that are targeted, 0-3 ground 4-7 air front to back
    public List<int[]> TargetGroups { get { return targetGroups; } }

    private String name;
    private int cost;
    
    public String Name { get { return name; } }
    public int Cost { get { return cost; } }

    // animation process tracking
    private bool running;
    private Animation activeAnimation;
    public bool Running { get { return running; } }

    private CharacterScript user;
    public List<CharacterScript> Targets { get; set; }

    public TurnMove(String name, int cost, CharacterScript user, MoveEffect effect, Animation animation, List<int[]> targetType) {
        running = false;
        this.name = name;
        this.cost = cost;
        this.user = user;
        this.moveEffect = effect;
        this.animationBlueprint = animation;
        this.targetGroups = targetType;
    }

    // executes the move's animation, ending with the mechanical effect
    public void Run() {
        running = true;
        activeAnimation = animationBlueprint.Copy();
    }

    public void Update() {
        activeAnimation.Update();
        if(activeAnimation.Complete) {
            running = false;
            moveEffect(user, Targets);
        }
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
