using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySelect : Menu
{
    public enum SelectionType {
        Ally,
        Adjacent,
        Any,
    }

    public List<ButtonScript> AllButtons;

    private TurnMove selectingMove;

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
