using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense
{
    private bool frail; // true: breaks after one attack, false: stays for entire turn
    public bool Frail { get { return frail; } }
    private Aim direction;
    public Aim Direction { get { return direction; } }

    public delegate void BlockEfect(Attack attack);
    public BlockEfect OnBlock { get; set; } // allow special effects

    public Defense(Aim direction, bool frail) {
        this.direction = direction;
        this.frail = frail;
    }


}
