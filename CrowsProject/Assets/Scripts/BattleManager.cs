using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject menus; // activate / deactivate all menus
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

    // turn execution
    List<TurnMove> turnOrder;
    private byte currentMove;
    private float waitBetweenMoves;
    private const float PAUSE_BETWEEN_MOVES = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Setup(1, 2);
        players = new CharacterScript[1];
        enemies = new CharacterScript[2];
        fliers = new CharacterScript[2];

        turnOrder = new List<TurnMove>();
        
        players[0] = GameObject.Find("Hunter").GetComponent<HunterScript>();

        // temp
        GameObject enemy = Instantiate(characterPrefab);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = new Vector3(5, 0, 0);
        enemies[0] = enemy.GetComponent<CharacterScript>();

        enemy = Instantiate(characterPrefab);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = new Vector3(8, 0, 0);
        enemies[1] = enemy.GetComponent<CharacterScript>();
    }

    // sets up this encounter
    public void Setup(int numPlayers, int numEnemies) {
        
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
                        menus.SetActive(true);
                        // add ability points
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
        menus.SetActive(false);

        // determine turn order
        // temp: players then enemies
        ChooseMoves();
        turnOrder.Clear();
        foreach(CharacterScript player in players) {
            if(player.SelectedMove != null) {
                turnOrder.Add(player.SelectedMove);
            }
        }
        foreach(CharacterScript enemy in enemies) {
            if(enemy.SelectedMove != null) {
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
            enemy.SelectMove("Attack", new List<CharacterScript>(players));
        }
    }
}
