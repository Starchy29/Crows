using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterScript
{
    public List<TurnMove> ViewMoves() { // must not change the list
        return moveList;
    }
}
