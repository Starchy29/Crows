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

[Flags]
public enum TargetType { // powers of 2 enable enums to hold multiple target types at once
    Self = 1,
    Ally = 2,
    Aerial = 4,
    Grounded = 8,
    Forward = 16,
    AllGround = 32
}

// defines how a move works and also tracks its animation progress
public class TurnMove
{
    // move definition
    private readonly Animation animationBlueprint;
    public delegate void MoveEffect(CharacterScript user, List<CharacterScript> targets);
    private MoveEffect moveEffect;

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

    public TurnMove(String name, int cost, CharacterScript user, MoveEffect effect, Animation animation) {
        running = false;
        this.name = name;
        this.cost = cost;
        this.user = user;
        this.moveEffect = effect;
        this.animationBlueprint = animation;
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
}
