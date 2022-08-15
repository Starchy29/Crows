using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterScript : CharacterScript
{
    protected override void AddMoves() {
        TurnMove heal = new TurnMove("Heal", this,
            (CharacterScript user, List<CharacterScript> targets) => {
                foreach(CharacterScript character in targets) {
                    character.Heal(2); 
                }
            },
            new Animation(),
            null // targets allies
        );
        heal.RequiredPosition = 0;
        moveList.Add(heal);
    }

    // button move selectors
    public void SelectAttack() {
        if(SelectMove("Attack")) {
            Global.Inst.EnemySelectMenu.OpenAndSetup(SelectedMove, Global.Inst.HunterMenu);
        }
    }

    public void SelectHeal() {
        if(SelectMove("Heal")) {
            Global.Inst.EnemySelectMenu.OpenAndSetupAlly(SelectedMove, Global.Inst.HunterMenu, false); // target any ally
        }
    }
}
