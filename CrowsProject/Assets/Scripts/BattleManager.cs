using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    private bool IsMoveSelect = true; // false: turn animation is playing

    private int abilityPoints;

    // character positions
    private CharacterScript[] players; // the player's characters
    private CharacterScript[] enemies;
    private CharacterScript[] fliers; // aerial enemies
    public CharacterScript[] Players { get { return players; } }
    public CharacterScript[] Enemies { get { return enemies; } }
    public CharacterScript[] Fliers { get { return fliers; } }
    private Vector3[] playerPositions;
    private Vector3[] enemyPositions;
    private Vector3[] flierPositions;

    // turn execution
    List<TurnMove> turnOrder;
    private byte currentMove;
    private float waitBetweenMoves;
    private const float PAUSE_BETWEEN_MOVES = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        players = new CharacterScript[4];
        enemies = new CharacterScript[4];
        fliers = new CharacterScript[4];

        turnOrder = new List<TurnMove>();

        playerPositions = new Vector3[4];
        for(int i = 0; i < 4; i++) {
            playerPositions[i] = new Vector3(-1.5f - 2.2f * i, -2, 0);
        }

        players[0] = Global.Inst.Cultist;
        players[1] = Global.Inst.Hunter;
        players[2] = Global.Inst.Demon;
        players[3] = Global.Inst.Witch;

        // position players and their selectors
        List<ButtonScript> selectButtons = Global.Inst.CharacterSelectMenu.Buttons;
        for(int i = 0; i < players.Length; i++) {
            Vector3 pos = playerPositions[i];
            players[i].gameObject.transform.position = pos;
            pos.y += 2;
            selectButtons[i].gameObject.transform.position = pos;
            selectButtons[i].SetBox();
        }

        // temp
        GameObject enemy = Instantiate(characterPrefab);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = new Vector3(5, 0, 0);
        enemies[0] = enemy.GetComponent<CharacterScript>();

        enemy = Instantiate(characterPrefab);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = new Vector3(8, 0, 0);
        enemies[1] = enemy.GetComponent<CharacterScript>();

        // make the battle start with a pause so the enemy can choose moves
        IsMoveSelect = false;
        currentMove = 0;
        waitBetweenMoves = PAUSE_BETWEEN_MOVES;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMoveSelect) {

        } else { // animation phase
            if(waitBetweenMoves > 0) {
                // pausing
                waitBetweenMoves -= Time.deltaTime;
                if(waitBetweenMoves <= 0) {
                    currentMove++;
                    if(currentMove >= turnOrder.Count) {
                        // end animation phase
                        IsMoveSelect = true;
                        ChooseMoves(); // can make decisions based on what player selected this turn
                        foreach(CharacterScript character in players) { // clear selected moves
                            character.SelectMove(null);
                        }
                        // add ability points
                        Global.Inst.CharacterSelectMenu.Open();
                    } else {
                        // run next move
                        turnOrder[currentMove].Run();
                    }
                }
            }
            else if(!turnOrder[currentMove].Running) { // check for turn end
                // begin pause that leads into next move
                waitBetweenMoves = PAUSE_BETWEEN_MOVES;
            }
            else {
                turnOrder[currentMove].Update();
            }
        }
    }

    // click event for when the player locks in their moves for the turn
    public void ConfirmTurn() {
        // close menus
        Global.Inst.CharacterSelectMenu.Close();

        // determine turn order
        // temp: players then enemies
        turnOrder.Clear();
        foreach(CharacterScript player in players) {
            if(player.SelectedMove != null) {
                turnOrder.Add(player.SelectedMove);
            }
        }
        foreach(CharacterScript enemy in enemies) {
            if(enemy != null && enemy.SelectedMove != null) {
                turnOrder.Add(enemy.SelectedMove);
            }
        }
        // add fliers later

        // begin animation process
        IsMoveSelect = false;
        currentMove = 0;
        waitBetweenMoves = 0;
        turnOrder[currentMove].Run();
    }

    // enemy AI
    private void ChooseMoves() {
        foreach(CharacterScript enemy in enemies) {
            if(enemy == null) {
                continue;
            }

            enemy.SelectMove("Attack");
            enemy.SelectedMove.Targets = new List<CharacterScript>() { players[1] };
        }
    }
}
