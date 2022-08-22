using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private ButtonScript[] enemyButtons;

    private bool IsMoveSelect = true; // false: turn animation is playing

    public int AbilityPoints { get; set; }

    // character positions
    private CharacterScript[] players; // the player's characters
    private CharacterScript[] enemies;
    private CharacterScript[] fliers; // aerial enemies
    public CharacterScript[] Players { get { return players; } }
    public CharacterScript[] Enemies { get { return enemies; } }
    public CharacterScript[] Fliers { get { return fliers; } }
    public readonly Vector3[] playerPositions = new Vector3[4];
    public readonly Vector3[] enemyPositions = new Vector3[4];
    public Vector3[] flierPositions = new Vector3[4];

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

        players[0] = Global.Inst.Cultist;
        players[1] = Global.Inst.Hunter;
        players[2] = Global.Inst.Demon;
        players[3] = Global.Inst.Witch;

        // Spawn in enemies
        GameObject enemy = Instantiate(characterPrefab);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = new Vector3(5, 0, 0);
        enemies[0] = enemy.GetComponent<CharacterScript>();

        enemy = Instantiate(characterPrefab);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
        enemy.transform.position = new Vector3(8, 0, 0);
        enemies[1] = enemy.GetComponent<CharacterScript>();

        // position players, enemies, and their selectors
        const float charY = -2f;
        const float selectorRise = 1.5f; // distance above characters
        const float fliersRise = 3f; // distance above grounded enemies
        const float characterGap = 1.5f;
        for(int i = 0; i < 4; i++) {
            playerPositions[i] = new Vector3(-2.5f - characterGap * i, charY, 0);
            enemyPositions[i] = new Vector3(2.5f + characterGap * i, charY, 0);
            flierPositions[i] = new Vector3(2.5f + characterGap * i, charY + fliersRise, 0);
        }

        List<ButtonScript> selectButtons = Global.Inst.CharacterSelectMenu.Buttons;
        for(int i = 0; i < players.Length; i++) {
            Vector3 playPos = playerPositions[i];
            if(players[i] != null) {
                players[i].gameObject.transform.position = playPos;
            }

            playPos.y += selectorRise;
            selectButtons[i].gameObject.transform.position = playPos;
            selectButtons[i].SetBox();

            Vector2 enemyPos = enemyPositions[i];
            if(enemies[i] != null) {
                enemies[i].gameObject.transform.position = enemyPos;
            }

            enemyPos.y += selectorRise;
            enemyButtons[i].gameObject.transform.position = enemyPos;
            enemyButtons[i].SetBox();

            Vector2 flierPos = flierPositions[i];
            if(fliers[i] != null) {
                fliers[i].gameObject.transform.position = flierPos;
            }

            flierPos.y += selectorRise;
            enemyButtons[i + 4].gameObject.transform.position = flierPos;
            enemyButtons[i + 4].SetBox();
        }

        // make the battle start with a pause so the enemy can choose moves
        IsMoveSelect = false;
        currentMove = 0;
        waitBetweenMoves = PAUSE_BETWEEN_MOVES;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMoveSelect) {
            //...?
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
                        AbilityPoints += 7;
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
            enemy.SelectedMove.Targets = new List<CharacterScript>() { players[Random.Range(0, 4)] };
        }
    }
}
