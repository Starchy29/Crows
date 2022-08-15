using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterScript : CharacterScript
{
    protected override void AddMoves() {
        moveList.Add(new TurnMove("Heal", 0, this,
            (CharacterScript user, List<CharacterScript> targets) => {
                foreach(CharacterScript character in targets) {
                    character.Heal(2); 
                }
            },
            new Animation(),
            null
        ));

    }

    // button move selectors
    public void SelectAttack() {
        SelectMove("Attack");
        Global.Inst.EnemySelectMenu.OpenAndSetup(SelectedMove, Global.Inst.HunterMenu);
    }

    public void SelectHeal() {
        SelectMove("Heal");
        Global.Inst.EnemySelectMenu.OpenAndSetupAlly(SelectedMove, Global.Inst.HunterMenu, false);
    }
}
