using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterScript : CharacterScript
{
    protected override void AddMoves() {
        moveList.Add(new TurnMove("Heal", 0, this,
            (CharacterScript user, List<CharacterScript> targets) => {
                user.Heal(2);
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
        //manager.gameObject.GetComponent<ButtonEvents>().ReturnToCharacterSelect();
    }
}
