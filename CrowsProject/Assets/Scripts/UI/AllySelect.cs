using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySelect : Menu
{
    public enum SelectionType {
        Ally, // any except self
        Adjacent,
        Any,
    }

    public List<ButtonScript> AllButtons; // cultist, hunter, demon, then witch

    //public delegate void NormalEvent();
    //public NormalEvent OnSelect;

    private TurnMove selectingMove; // the move that this menu is assigning targets to
    private CharacterScript user;

    public void OpenAndSetup(TurnMove selector, Menu opener, SelectionType selection, int userSlot) {
        selectingMove = selector;
        previous = opener;
        opener.Close();
        Open();

        foreach(ButtonScript button in buttons) {
            button.Deselect();
        }
        buttons.Clear();

        switch(selection) {
            case SelectionType.Adjacent:
                if(userSlot - 1 >= 0) {
                    buttons.Add(AllButtons[userSlot - 1]);
                }
                if(userSlot + 1 < 4) {
                    buttons.Add(AllButtons[userSlot + 1]);
                }
                break;
            case SelectionType.Ally:
                for(int i = 0; i < 4; i++) {
                    if(i == userSlot) {
                        continue;
                    }
                    buttons.Add(AllButtons[i]);
                }
                break;
            case SelectionType.Any:
                foreach(ButtonScript button in AllButtons) {
                    buttons.Add(button);
                }
                break;
        }

        FullDeselect();
        Selected = buttons[0];
        Selected.Select();
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.Find("Input Manager").GetComponent<InputManager>();
        foreach(ButtonScript button in AllButtons) {
            button.Group = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(input.ConfirmJustPressed()) {
            selectingMove.Targets = new List<CharacterScript>();
            int index = AllButtons.IndexOf(Selected);
            selectingMove.Targets.Add(Global.Inst.BattleManager.Players[index]);
            

            // if swap move, swap now
            if(selectingMove.SwapFunction != null) {
                selectingMove.ExecuteSwap();
            }
            //if(OnSelect != null) {
            //    OnSelect();
            //    OnSelect = null;
            //}

            // close menu
            Close();
            Global.Inst.CharacterSelectMenu.Open();
        }
        else if(input.CancelJustPressed() && previous != null) {
            // go to previous menu
            Close();
            previous.Open();
            // deselect this move
            if(previous == Global.Inst.CultistMenu) {
                Global.Inst.Cultist.Deselect();
            }
            else if(previous == Global.Inst.HunterMenu) {
                Global.Inst.Hunter.Deselect();
            }
            else if(previous == Global.Inst.DemonMenu) {
                Global.Inst.Demon.Deselect();
            }
            else if(previous == Global.Inst.WitchMenu) {
                Global.Inst.Witch.Deselect();
            }
        }
        else if(input.JustPressed(Direction.Up)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Up, buttons);
            Selected.Select();
        }
        else if(input.JustPressed(Direction.Down)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Down, buttons);
            Selected.Select();
        }
        else if(input.JustPressed(Direction.Left)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Left, buttons);
            Selected.Select();
        }
        else if(input.JustPressed(Direction.Right)) {
            FullDeselect();
            Selected = Selected.GetClosestNeighbor(Direction.Right, buttons);
            Selected.Select();
        }
    }

    private void FullDeselect() {
        foreach(ButtonScript button in AllButtons) {
            button.Deselect();
        }
    }
}
