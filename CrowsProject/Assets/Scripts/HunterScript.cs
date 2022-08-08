using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterScript : CharacterScript
{
    protected override void AddMoves() {
        moveList.Add(new TurnMove("Heal", 0, this,
            (CharacterScript user, List<CharacterScript> targets) => {
                this.Health += 2;
            },
            new Animation()
        ));

    }

    // button move selectors
    public void SelectAttack() {
        SelectMove("Attack", new List<CharacterScript>(){ manager.Enemies[0] });
    }

    public void SelectHeal() {
        SelectMove("Heal");
    }
}
