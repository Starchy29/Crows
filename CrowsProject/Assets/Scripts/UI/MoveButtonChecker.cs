using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// grays out move buttons that are not valid
public class MoveButtonChecker : MonoBehaviour
{
    [SerializeField] private CharacterScript user;
    [SerializeField] private List<String> names; // indices must line up with the order of buttons on the attack menu

    public void CheckMoves() {
        for(int i = 0; i < names.Count; i++) {
            if(user.IsMoveUsable(names[i])) {
                gameObject.GetComponent<Menu>().Buttons[i].GetComponent<ButtonScript>().regularColor = Color.black;
            } else {
                gameObject.GetComponent<Menu>().Buttons[i].GetComponent<ButtonScript>().regularColor = Color.gray;
            }
        }
    }
}
