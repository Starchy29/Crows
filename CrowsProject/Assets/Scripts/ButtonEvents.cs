using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// collection of events for menu buttons
public class ButtonEvents : MonoBehaviour
{
    // character select menu
    public void SelectCultist() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.CultistMenu.Open();
        Global.Inst.CultistMenu.GetComponent<MoveButtonChecker>().CheckMoves();
        Global.Inst.Cultist.SelectMove(null);
    }

    public void SelectHunter() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.HunterMenu.Open();
        Global.Inst.HunterMenu.GetComponent<MoveButtonChecker>().CheckMoves();
        Global.Inst.Hunter.SelectMove(null);
    }

    public void SelectDemon() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.DemonMenu.Open();
        Global.Inst.DemonMenu.GetComponent<MoveButtonChecker>().CheckMoves();
        Global.Inst.Demon.SelectMove(null);
    }

    public void SelectWitch() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.WitchMenu.Open();
        Global.Inst.WitchMenu.GetComponent<MoveButtonChecker>().CheckMoves();
        Global.Inst.Witch.SelectMove(null);
    }
}
