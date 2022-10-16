using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedMoveDisplay : MonoBehaviour
{
    public static GameObject MoveDisplayPrefab;

    // creates a copy of a prefab, populates it with the appropriate imagery, then sets the move's Display to it so it can be deleted later
    public static void CreateMoveDisplay(TurnMove move, CharacterScript user) {
        GameObject display = Instantiate(MoveDisplayPrefab);

        move.Display = display;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
