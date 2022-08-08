using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchScript : CharacterScript
{
    protected override void AddMoves()
    {
        
    }

    // button move selectors
    public void SelectAttack()
    {
        SelectMove("Attack", new List<CharacterScript>() { manager.Enemies[0] });
    }
}
