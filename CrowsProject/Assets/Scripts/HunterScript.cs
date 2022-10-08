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
        swap.SwapFunction = (CharacterScript user, List<CharacterScript> targets) => {
            CharacterScript[] newOrder = new CharacterScript[4];
            Global.Inst.BattleManager.Players.CopyTo(newOrder, 0);
            int thisIndex = Global.Inst.BattleManager.GetPlayerIndex(user);
            int otherIndex = Global.Inst.BattleManager.GetPlayerIndex(SelectedMove.Targets[0]);

            CharacterScript swapper = Global.Inst.BattleManager.Players[thisIndex];
            newOrder[otherIndex] = swapper;
            newOrder[thisIndex] = Global.Inst.BattleManager.Players[otherIndex];
            Global.Inst.BattleManager.SwapCharacters(newOrder);
        };
        moveList.Add(swap);
    }

    // button move selectors
    public void SelectAttack() {
        if(SelectMove("Attack")) {
            Global.Inst.EnemySelectMenu.OpenAndSetup(SelectedMove, Global.Inst.HunterMenu);
        }
    }

    //public void SelectHeal() {
    //    if(SelectMove("Heal")) {
    //        List<CharacterScript> listPlayers = new List<CharacterScript>(Global.Inst.BattleManager.Players);
    //        Global.Inst.AllySelectMenu.OpenAndSetup(SelectedMove, Global.Inst.HunterMenu, AllySelect.SelectionType.Any, listPlayers.IndexOf(Global.Inst.Hunter));
    //    }
    //}

    public void SelectHeal() {
        if(SelectMove("Swap")) {
            Global.Inst.AllySelectMenu.OpenAndSetup(SelectedMove, Global.Inst.HunterMenu, AllySelect.SelectionType.Adjacent, Global.Inst.BattleManager.GetPlayerIndex(this));
        }
    }
}
