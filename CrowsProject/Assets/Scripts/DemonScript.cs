using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : CharacterScript
{
    protected override void AddMoves()
    {

    }

    // button move selectors
    public void SelectAttack()
    {
        SelectMove("Attack");
    }
}
