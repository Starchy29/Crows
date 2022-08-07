using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a character in combat
public class CharacterScript : MonoBehaviour
{
    protected BattleManager manager;

    // health
    [SerializeField] private int maxHealth;
    protected int health;
    public int Health {
        get { return health; }
        set {
            if(!IsAlive) {
                return;
            }
            if(value > maxHealth) {
                health = maxHealth;
            }
            else if(value < 0) {
                health = 0;
            }
            else {
                health = value;
            }
            UpdateHealthBar();
        }
    }
    public bool IsAlive { get { return health > 0; } }

    // move selection
    private TurnMove selectedMove;
    protected List<TurnMove> moveList;
    public TurnMove SelectedMove { get { return selectedMove; } }
    public void SelectMove(String name, List<CharacterScript> targets = null) { // can use null to select none
        selectedMove = moveList.Find((TurnMove check) => { return check.Name.Equals(name); });
        if(selectedMove != null) {
            selectedMove.Targets = targets;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        manager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        moveList = new List<TurnMove>();
        selectedMove = null;
        AddMoves();
        
        // temp
        moveList.Add(new TurnMove("Attack", 0, this, 
            (CharacterScript user, List<CharacterScript> targets) => {
                targets[0].Health -= 1;
            },
            new Animation()
        ));
    }

    protected virtual void AddMoves() { } // sub classes can add their specific moves

    private void UpdateHealthBar() {
        GameObject hpBar = gameObject.transform.GetChild(0).gameObject;
        GameObject hpBack = gameObject.transform.GetChild(1).gameObject;
        Vector3 newScale = hpBack.transform.localScale;
        newScale.x *= (float)health / maxHealth;
        float shift = (hpBack.transform.localScale.x - newScale.x) / 2; // keep left-justified
        hpBar.transform.localScale = newScale;
        Vector3 newPos = hpBack.transform.position;
        newPos.x -= shift;
        hpBar.transform.position = newPos;
    }
}
