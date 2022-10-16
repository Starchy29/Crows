using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private ButtonScript[] enemyButtons;

    [SerializeField] private ButtonScript[] moveSelectButtons; // cultist, hunter, demon, witch
    [SerializeField] private ButtonScript[] allySelectButtons;
    private Dictionary<CharacterScript, ButtonScript> allyToMoveSelect;
    private Dictionary<CharacterScript, ButtonScript> allyToAllySelect;

    private bool IsMoveSelect = true; // false: turn animation is playing, might be useless

    private CharacterScript[] beforeSwap; // if not null, means the player's one swap for the turn has been used. Stores the formation before the swap happened for undo
    public bool HasSwapped { get { return beforeSwap != null; } }
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
    private const float selectorRise = 1.5f; // distance above characters

    // turn execution
    private TurnMove currentMove;
    private float waitBetweenMoves;
    private const float PAUSE_BETWEEN_MOVES = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        players = new CharacterScript[4];
        enemies = new CharacterScript[4];
        fliers = new CharacterScript[4];

        players[0] = Global.Inst.Cultist;
        players[1] = Global.Inst.Hunter;
        players[2] = Global.Inst.Demon;
        players[3] = Global.Inst.Witch;

        allyToMoveSelect = new Dictionary<CharacterScript, ButtonScript>();
        allyToMoveSelect[Global.Inst.Cultist] = moveSelectButtons[0];
        allyToMoveSelect[Global.Inst.Hunter] = moveSelectButtons[1];
        allyToMoveSelect[Global.Inst.Demon] = moveSelectButtons[2];
        allyToMoveSelect[Global.Inst.Witch] = moveSelectButtons[3];

        allyToAllySelect = new Dictionary<CharacterScript, ButtonScript>();
        allyToAllySelect[Global.Inst.Cultist] = allySelectButtons[0];
        allyToAllySelect[Global.Inst.Hunter] = allySelectButtons[1];
        allyToAllySelect[Global.Inst.Demon] = allySelectButtons[2];
        allyToAllySelect[Global.Inst.Witch] = allySelectButtons[3];

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
        const float fliersRise = 3f; // distance above grounded enemies
        const float characterGap = 1.5f;
        for(int i = 0; i < 4; i++) {
            playerPositions[i] = new Vector3(-2.5f - characterGap * i, charY, 0);
            enemyPositions[i] = new Vector3(2.5f + characterGap * i, charY, 0);
            flierPositions[i] = new Vector3(2.5f + characterGap * i, charY + fliersRise, 0);
        }

        AlignPlayers();

        List<ButtonScript> selectButtons = Global.Inst.CharacterSelectMenu.Buttons;
        List<ButtonScript> targetButtons = Global.Inst.AllySelectMenu.AllButtons;
        for(int i = 0; i < enemies.Length; i++) {
            Vector2 enemyPos = enemyPositions[i];
            if(enemies[i] != null) {
                enemies[i].gameObject.transform.position = enemyPos;
            }

            enemyPos.y += selectorRise;
            enemyButtons[i].SetPosition(enemyPos);

            Vector2 flierPos = flierPositions[i];
            if(fliers[i] != null) {
                fliers[i].gameObject.transform.position = flierPos;
            }

            flierPos.y += selectorRise;
            enemyButtons[i + 4].SetPosition(flierPos);
        }

        // make the battle start with a pause so the enemy can choose moves
        IsMoveSelect = false;
        currentMove = null;
        waitBetweenMoves = PAUSE_BETWEEN_MOVES;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMoveSelect) {
            //...?
        } else { // animation phase
            if(waitBetweenMoves > 0) { // pausing between moves
                waitBetweenMoves -= Time.deltaTime;
                if(waitBetweenMoves <= 0) {
                    // done with pause
                    if(currentMove == null) { 
                        // end animation phase
                        IsMoveSelect = true;
                        ChooseMoves(); // can make decisions based on what player selected this turn
                        foreach(CharacterScript character in players) { // clear selected moves
                            character.SelectMove(null);
                        }
                        AbilityPoints += 6;
                        beforeSwap = null; // allow another swap move

                        // reset stats
                        for(int i = 0; i < 4; i++) {
                            players[i]?.OnTurnEnd();
                            enemies[i]?.OnTurnEnd();
                            fliers[i]?.OnTurnEnd();
                        }

                        Global.Inst.CharacterSelectMenu.Open();
                    } else { 
                        // run next move
                        currentMove.Run();
                    }
                }
            }
        }
    }

    // called when a move is completed, starts the pause before running the next move
    public void CompleteMove() {
        waitBetweenMoves = PAUSE_BETWEEN_MOVES;
        currentMove = currentMove.NextMove;
    }

    // click event for when the player locks in their moves for the turn
    public void ConfirmTurn() {
        // close menus
        Global.Inst.CharacterSelectMenu.Close();

        // determine turn order
        // temp: players then enemies
        List<TurnMove> moveOrder = new List<TurnMove>();
        foreach(CharacterScript player in players) {
            if(player.SelectedMove != null && player.SelectedMove.SwapFunction == null) { // don't do anything if the move is a swap
                moveOrder.Add(player.SelectedMove);
            }
        }
        foreach(CharacterScript enemy in enemies) {
            if(enemy != null && enemy.SelectedMove != null) {
                moveOrder.Add(enemy.SelectedMove);
            }
        }
        currentMove = moveOrder[0];
        for(int i = 1; i < moveOrder.Count; i++) {
            moveOrder[i - 1].NextMove = moveOrder[i];
        }
        moveOrder[moveOrder.Count - 1].NextMove = null;
        // add fliers later

        // begin animation process
        IsMoveSelect = false;
        waitBetweenMoves = 0;
        currentMove.Run();
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

    // helper
    public int GetPlayerIndex(CharacterScript player) {
        for(int i = 0; i < players.Length; i++) {
            if(players[i] == player) {
                return i;
            }
        }

        return -1;
    }

    // moves the characters into the indicated positions
    public void SwapCharacters(CharacterScript[] newOrder) {
        beforeSwap = players;
        players = newOrder;
        AlignPlayers();
    }

    public void UndoSwap() {
        players = beforeSwap;
        beforeSwap = null;
        AlignPlayers();
    }

    // puts the players in their appropriate position and moves their buttons
    private void AlignPlayers() {
        for(int i = 0; i < players.Length; i++) {
            players[i].transform.position = playerPositions[i];
            Vector3 buttonPos = playerPositions[i];
            buttonPos.y += selectorRise;
            allyToAllySelect[players[i]].SetPosition(buttonPos);
            allyToMoveSelect[players[i]].SetPosition(buttonPos);
        }
    }
}
