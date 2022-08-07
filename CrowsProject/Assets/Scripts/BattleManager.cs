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
    public CharacterScript[] players { get; set; } // the player's characters
    public CharacterScript[] enemies { get; set; }
    public CharacterScript[] fliers { get; set; } // aerial enemies

    // turn execution
    List<TurnMove> turnOrder;
    private byte currentMove;
    private float waitBetweenMoves;
    private const float PAUSE_BETWEEN_MOVES = 1;

    // Start is called before the first frame update
    void Start()
    {
        Setup(1, 1);

        turnOrder = new List<TurnMove>();

        // temp
        GameObject player = Instantiate(characterPrefab);
        player.transform.position = new Vector3(-5, 0, 0);
        players[0] = player.GetComponent<CharacterScript>();

        GameObject enemy = Instantiate(characterPrefab);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = new Vector3(5, 0, 0);
        enemies[0] = enemy.GetComponent<CharacterScript>();
    }

    // sets up this encounter
    public void Setup(int numPlayers, int numEnemies) {
        players = new CharacterScript[numPlayers];
        enemies = new CharacterScript[numEnemies];
        fliers = new CharacterScript[numEnemies];
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
                        // AI chooses moves for next turn
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
        players[0].SelectMove("Attack", new List<CharacterScript>(){ enemies[0] });
        enemies[0].SelectMove("Attack", new List<CharacterScript>() { players[0] });
        // close menus

        // determine turn order
        // temp: players then enemies
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
}
