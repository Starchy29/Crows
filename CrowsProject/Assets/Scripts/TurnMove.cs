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

    // move animation process tracking
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
