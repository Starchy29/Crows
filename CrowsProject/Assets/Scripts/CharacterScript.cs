using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a character in combat
public class CharacterScript : MonoBehaviour
{
    private HealthBar hpBar;

    // health
    [SerializeField] private uint maxHealth;
    protected uint health;
    public uint Health { get { return health; } }
    public bool IsAlive { get { return health > 0; } }

    // move selection
    private TurnMove selectedMove;
    protected List<TurnMove> moveList;
    public TurnMove SelectedMove { get { return selectedMove; } }
    public void SelectMove(String name) { // can use null to select none
        selectedMove = moveList.Find((TurnMove check) => { return check.Name.Equals(name); });
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        hpBar = transform.GetChild(0).GetComponent<HealthBar>();
        moveList = new List<TurnMove>();
        selectedMove = null;
        AddMoves();
        
        // temp
        moveList.Add(new TurnMove("Attack", 0, this, 
            (CharacterScript user, List<CharacterScript> targets) => {
                targets[0].TakeDamage(1);
            },
            new Animation(),
            TurnMove.GroundTarget
        ));
    }

    public void Heal(uint amount) {
        health += amount;
        if(health > maxHealth) {
            health = maxHealth;
        }
        hpBar.Increase(amount, (float)health / maxHealth);
    }

    public void TakeDamage(uint amount) {
        if(amount > health) {
            health = 0;
        } else {
            health -= amount;
        }
        hpBar.Reduce(amount, (float)health / maxHealth);
    }

    protected virtual void AddMoves() { } // sub classes can add their specific moves
}
