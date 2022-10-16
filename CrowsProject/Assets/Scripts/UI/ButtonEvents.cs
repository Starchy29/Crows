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
        Global.Inst.Cultist.Deselect();
    }

    public void SelectHunter() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.HunterMenu.Open();
        Global.Inst.HunterMenu.GetComponent<MoveButtonChecker>().CheckMoves();
        Global.Inst.Hunter.Deselect();
    }

    public void SelectDemon() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.DemonMenu.Open();
        Global.Inst.DemonMenu.GetComponent<MoveButtonChecker>().CheckMoves();
        Global.Inst.Demon.Deselect();
    }

    public void SelectWitch() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.WitchMenu.Open();
        Global.Inst.WitchMenu.GetComponent<MoveButtonChecker>().CheckMoves();
        Global.Inst.Witch.Deselect();
    }
}
