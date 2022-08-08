using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a group of buttons where only one is selected at a time
public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private List<ButtonScript> buttons;
    private InputManager input;
    public ButtonScript Selected { get; set; }
    public List<ButtonScript> Buttons { get { return buttons; } }

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.Find("Input Manager").GetComponent<InputManager>();
        foreach(ButtonScript button in buttons) {
            button.Group = this;
            button.Deselect();
        }
        Selected = buttons[0];
        Selected.Select();
    }

    private void Update() {
        if(input.ConfirmJustPressed()) {
            Selected.Click();
        }
        else if(input.JustPressed(Direction.Up)) {
            Selected.Deselect();
            Selected = Selected.GetClosestNeighbor(Direction.Up);
            Selected.Select();
        }
        else if(input.JustPressed(Direction.Down)) {
            Selected.Deselect();
            Selected = Selected.GetClosestNeighbor(Direction.Down);
            Selected.Select();
        }
        else if(input.JustPressed(Direction.Left)) {
            Selected.Deselect();
            Selected = Selected.GetClosestNeighbor(Direction.Left);
            Selected.Select();
        }
        else if(input.JustPressed(Direction.Right)) {
            Selected.Deselect();
            Selected = Selected.GetClosestNeighbor(Direction.Right);
            Selected.Select();
        }
    }
}
