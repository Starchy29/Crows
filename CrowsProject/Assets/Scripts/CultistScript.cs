using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistScript : CharacterScript
{
    public bool HasSwapped; // prevent multiple swaps in a turn

    protected override void AddMoves()
    {
    }

    // button move selectors
    public void SelectAttack()
    {
        SelectMove("Attack");
    }
}
