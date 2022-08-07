using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a group of buttons where only one is selected at a time
public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private List<ButtonScript> buttons;
    public ButtonScript Selected { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Selected = null;
        foreach(ButtonScript button in buttons) {
            button.Group = this;
        }
    }
}
