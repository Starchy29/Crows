using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// special menu that changes which buttons are enabled
public class EnemySelection : Menu
{
    [SerializeField] private ButtonScript[] buttonSlots; // 0-3 ground, 4-7 air, front to back
    [SerializeField] private ButtonScript[] allyButtons; // 0-3 is cultist, hunter, demon, witch
    private List<List<ButtonScript>> buttonGroups;
    private TurnMove selectingMove;
    private bool allySelect; // false: select enemy
    private List<ButtonScript> orderedAllies;

    void Start()
    {
        input = GameObject.Find("Input Manager").GetComponent<InputManager>();
        foreach(ButtonScript button in buttonSlots) {
            button.Group = this;
            if(button != Selected) { // Start() is called after the first OpenAndSetup()
                button.Deselect();
            }
        }
    }

    // sets up the enemy buttons to match the selection type of the selected move
    public void OpenAndSetup(TurnMove selector, Menu opener) {
        allySelect = false;
        selectingMove = selector;
        previous = opener;
        opener.Close();
        Open();

        foreach(ButtonScript button in buttons) {
            button.Deselect();
        }
        buttons.Clear();

        // find which slots have enemies in them
        List<int> validSlots = new List<int>();
        for(int i = 0; i < 4; i++) {
            buttonSlots[i].ToolTips = new List<GameObject>(); // clear previous health bar connections

            CharacterScript enemy = Global.Inst.BattleManager.Enemies[i];
            if(enemy != null) {
                validSlots.Add(i);
                buttonSlots[i].ToolTips.Add(enemy.gameObject.transform.GetChild(0).gameObject); // hook up health bar viewer
            }

            // same process for aerial slots
            buttonSlots[i + 4].ToolTips = new List<GameObject>(); // clear previous health bar connections
            CharacterScript flier = Global.Inst.BattleManager.Fliers[i];
            if(flier != null) {
                validSlots.Add(i + 4);
                buttonSlots[i + 4].ToolTips.Add(enemy.gameObject.transform.GetChild(0).gameObject); // hook up health bar viewer
            }
        }

        // set up button selection groups
        buttonGroups = new List<List<ButtonScript>>();
        foreach(int[] group in selector.TargetGroups) {
            // filter out invalid slots
            List<int> approvedGroup = new List<int>();
            foreach(int slot in group) {
                if(validSlots.Contains(slot)) {
                    approvedGroup.Add(slot);
                }
            }
            if(approvedGroup.Count <= 0) {
                continue;
            }

            // add buttons now
            buttons.Add(buttonSlots[group[0]]); // the first button is the one that gets selected
            List<ButtonScript> addedGroup = new List<ButtonScript>();
            for(int i = 1; i < group.Length; i++) {
                addedGroup.Add(buttonSlots[group[i]]);
            }
            buttonGroups.Add(addedGroup);
        }

        Selected = buttons[0];
        Selected.Select();
    }

    // variant that selects allies. If user is null, it can select any of the four allies, else it targets the user only
    public void OpenAndSetupAlly(TurnMove selector, Menu opener, bool selectAll, CharacterScript user = null) {
        allySelect = true;
        selectingMove = selector;
        previous = opener;
        opener.Close();
        Open();

        foreach(ButtonScript button in buttons) {
            button.Deselect();
        }
        buttons.Clear();

        Global.Inst.CharacterSelectMenu.gameObject.SetActive(true);
        Global.Inst.CharacterSelectMenu.gameObject.GetComponent<Menu>().enabled = false; // disable regular menu
        Global.Inst.CharacterSelectMenu.gameObject.transform.GetChild(4).gameObject.SetActive(false); // hide ready button

        // setup ally button order
        orderedAllies = new List<ButtonScript>();
        foreach(CharacterScript character in Global.Inst.BattleManager.Players) {
            if(character is CultistScript) {
                orderedAllies.Add(allyButtons[0]);
            }
            else if(character is HunterScript) {
                orderedAllies.Add(allyButtons[1]);
            }
            else if(character is DemonScript) {
                orderedAllies.Add(allyButtons[2]);
            }
            else if(character is WitchScript) {
                orderedAllies.Add(allyButtons[3]);
            }
        }

        // add ally buttons
        buttonGroups = new List<List<ButtonScript>>();
        if(selectAll) {
            // highlight all four at once
            buttons.Add(allyButtons[0]);
            buttonGroups.Add(new List<ButtonScript>());
            buttonGroups[0].Add(allyButtons[1]);
            buttonGroups[0].Add(allyButtons[2]);
            buttonGroups[0].Add(allyButtons[3]);
        }
        else if(user != null) {
            // only choose self
            if(user is CultistScript) {
                buttons.Add(allyButtons[0]);
            }
            else if(user is HunterScript) {
                buttons.Add(allyButtons[1]);
            }
            else if(user is DemonScript) {
                buttons.Add(allyButtons[2]);
            }
            else if(user is WitchScript) {
                buttons.Add(allyButtons[3]);
            }
        }
        else {
            // choose any of the four
            buttons.AddRange(allyButtons);
        }

        Selected = buttons[0];
        FullSelect();
    }

    void Update() {
        if(input.ConfirmJustPressed()) {
            // pass targets to selected move
            if(allySelect) {
                // set character select back to normal
                Global.Inst.CharacterSelectMenu.gameObject.SetActive(false);
                Global.Inst.CharacterSelectMenu.gameObject.GetComponent<Menu>().enabled = true; // allow regular menu to work
                Global.Inst.CharacterSelectMenu.gameObject.transform.GetChild(4).gameObject.SetActive(true); // enable ready button
                foreach(ButtonScript button in allyButtons) {
                    button.Deselect();
                }

                selectingMove.Targets = new List<CharacterScript>();
                if(buttonGroups.Count > 0) {
                    // all are selected
                    selectingMove.Targets.Add(Global.Inst.Cultist);
                    selectingMove.Targets.Add(Global.Inst.Hunter);
                    selectingMove.Targets.Add(Global.Inst.Demon);
                    selectingMove.Targets.Add(Global.Inst.Witch);
                } else {
                    selectingMove.Targets.Add(Global.Inst.BattleManager.Players[orderedAllies.IndexOf(Selected)]);
                }
            } else {
                List<ButtonScript> chosenGroup = buttonGroups[Buttons.IndexOf(Selected)];
                chosenGroup.Add(Selected);
                selectingMove.Targets = new List<CharacterScript>();
                foreach(ButtonScript button in chosenGroup) {
                    int slot = Buttons.IndexOf(Selected);
                    if(slot < 4) {
                        selectingMove.Targets.Add(Global.Inst.BattleManager.Enemies[slot]);
                    } else {
                        selectingMove.Targets.Add(Global.Inst.BattleManager.Fliers[slot - 4]);
                    }
                }
            }

            // close menu
            Close();
            Global.Inst.CharacterSelectMenu.Open();
        }
        else if(input.CancelJustPressed() && previous != null) {
            // go to previous menu
            Close();
            previous.Open();
        }
        else if(input.JustPressed(Direction.Up)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Up, buttons);
            FullSelect();
        }
        else if(input.JustPressed(Direction.Down)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Down, buttons);
            FullSelect();
        }
        else if(input.JustPressed(Direction.Left)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Left, buttons);
            FullSelect();
        }
        else if(input.JustPressed(Direction.Right)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Right, buttons);
            FullSelect();
        }
    }

    private void FullDeselect() {
        foreach(ButtonScript button in buttonSlots) {
            button.Deselect();
        }
        foreach(ButtonScript button in allyButtons) {
            button.Deselect();
        }
    }

    private void FullSelect() {
        Selected.Select();
        if(buttonGroups.Count > 0) {
            int groupNumber = buttons.IndexOf(Selected);
            foreach(ButtonScript button in buttonGroups[groupNumber]) {
                button.Select();
            }
        }
    }
}
