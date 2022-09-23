using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterScript : CharacterScript
{
    public bool HasSwapped; // prevent multiple swaps in a turn

    protected override void AddMoves() {
        TurnMove heal = new TurnMove("Heal", this,
            (CharacterScript user, List<CharacterScript> targets) => {
                foreach(CharacterScript character in targets) {
                    character.Heal(2); 
                }
            },
            null // targets allies
        );
        moveList.Add(heal);

        TurnMove swap = new TurnMove("Swap", this,
            null,
            null // targets allies
        );
        moveList.Add(swap);
    }

    // button move selectors
    public void SelectAttack() {
        if(SelectMove("Attack")) {
            Global.Inst.EnemySelectMenu.OpenAndSetup(SelectedMove, Global.Inst.HunterMenu);
        }
    }

    public void SelectHeal() {
        if(SelectMove("Swap")) {
            List<CharacterScript> listPlayers = new List<CharacterScript>(Global.Inst.BattleManager.Players);
            Global.Inst.AllySelectMenu.OpenAndSetup(SelectedMove, Global.Inst.HunterMenu, AllySelect.SelectionType.Adjacent, listPlayers.IndexOf(Global.Inst.Hunter));
        }
    }
}
