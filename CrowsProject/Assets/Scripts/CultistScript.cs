using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistScript : CharacterScript
{
    protected override void AddMoves()
    {
        moveList.Add(new TurnMove("Kill", 0, this,
            (CharacterScript user, List<CharacterScript> targets) => {
                targets[0].Health -= 3;
            },
            new Animation()
        ));
    }

    // button move selectors
    public void SelectAttack()
    {
        SelectMove("Attack", new List<CharacterScript>() { manager.Enemies[0] });
    }

    public void SelectKill()
    {
        SelectMove("Kill", new List<CharacterScript>() { manager.Enemies[0] });
    }
}
