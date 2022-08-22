using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a character in combat
public class CharacterScript : MonoBehaviour
{
    private HealthBar hpBar;
    [SerializeField] private bool isAlly;
    public bool IsAlly { get { return isAlly; } }

    // health
    [SerializeField] private uint maxHealth;
    protected uint health;
    public uint Health { get { return health; } }
    public bool IsAlive { get { return health > 0; } }

    // move selection
    private TurnMove selectedMove;
    protected List<TurnMove> moveList;
    public TurnMove SelectedMove { get { return selectedMove; } }
    public bool SelectMove(String name) { // can use null to select none, returns true if succeeded
        if(name == null) {
            selectedMove = null;
            return true;
        }

        TurnMove chosen = moveList.Find((TurnMove check) => { return check.Name.Equals(name); });
        if(chosen.CanUse()) {
            selectedMove = chosen;
            Global.Inst.BattleManager.AbilityPoints -= selectedMove.Cost; // enemy moves should cost 0
            return true;
        }

        return false;
    }
    // undoes a move selection and refunds ability points
    public void Deselect() {
        if(selectedMove != null) {
            Global.Inst.BattleManager.AbilityPoints += selectedMove.Cost;
            selectedMove = null;
        }
    }
    public bool IsMoveUsable(String name) { // used to gray out unusable buttons
        return moveList.Find((TurnMove check) => { return check.Name.Equals(name); }).CanUse();
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
        moveList.Add(new TurnMove("Attack", this, 
            (CharacterScript user, List<CharacterScript> targets) => {
                targets[0].TakeDamage(1);
            },
            new Animation(),
            TurnMove.GroundTarget
        ));
    }

    // position in the line of characters
    public int GetPosition() {
        if(isAlly) {
            for(int i = 0; i < 4; i++) {
                if(Global.Inst.BattleManager.Players[i] == this) {
                    return i;
                }
            }
        } else {
            for(int i = 0; i < 4; i++) {
                if(Global.Inst.BattleManager.Enemies[i] == this
                    || Global.Inst.BattleManager.Fliers[i] == this
                ) {
                    return i;
                }
            }
        }

        return -1;
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
