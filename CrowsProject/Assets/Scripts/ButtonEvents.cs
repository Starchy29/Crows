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
        Global.Inst.Cultist.SelectMove(null);
    }

    public void SelectHunter() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.HunterMenu.Open();
        Global.Inst.Hunter.SelectMove(null);
    }

    public void SelectDemon() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.DemonMenu.Open();
        Global.Inst.Demon.SelectMove(null);
    }

    public void SelectWitch() {
        Global.Inst.CharacterSelectMenu.Close();
        Global.Inst.WitchMenu.Open();
        Global.Inst.Witch.SelectMove(null);
    }
}
