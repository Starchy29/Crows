using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    private Aim aim;
    public Aim AimType { get { return aim; } } 

    private uint damage;
    public uint Damage { get { return damage; } }

    public delegate void HitEffect(CharacterScript target);
    public HitEffect OnHit { get; set; }

    public Attack(uint damage, Aim aim = Aim.None) {
        this.damage = damage;
        this.aim = aim;
    }
}
