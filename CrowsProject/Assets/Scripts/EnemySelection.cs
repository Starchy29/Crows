using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// special menu that changes which buttons are enabled
public class EnemySelection : Menu
{
    [SerializeField] private ButtonScript[] buttonSlots; // 0-3 ground, 4-7 air, front to back
    private List<List<ButtonScript>> buttonGroups;
    private TurnMove selectingMove;

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
    // to do: check if there is an enemy in each slot
    public void OpenAndSetup(TurnMove selector, Menu opener) {
        selectingMove = selector;
        previous = opener;
        opener.Close();
        Open();

        foreach(ButtonScript button in buttons) {
            button.Deselect();
        }
        buttons.Clear();

        // find which slots have enmies in them
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

    void Update() {
        if(input.ConfirmJustPressed()) {
            // pass targets to selected move
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
            Selected = Selected.GetClosestNeighbor(Direction.Up);
            FullSelect();
        }
        else if(input.JustPressed(Direction.Down)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Down);
            FullSelect();
        }
        else if(input.JustPressed(Direction.Left)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Left);
            FullSelect();
        }
        else if(input.JustPressed(Direction.Right)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Right);
            FullSelect();
        }
    }

    private void FullDeselect() {
        foreach(ButtonScript button in buttonSlots) {
            button.Deselect();
        }
    }

    private void FullSelect() {
        int groupNumber = buttons.IndexOf(Selected);
        Selected.Select();
        foreach(ButtonScript button in buttonGroups[groupNumber]) {
            button.Select();
        }
    }
}
