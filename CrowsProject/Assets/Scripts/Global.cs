using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventType();

public class Global : MonoBehaviour
{
    private static Global inst;
    public static Global Inst { get { return inst; } }

    void Awake()
    {
        inst = this;
    }

    [SerializeField] private BattleManager battleManager;
    public BattleManager BattleManager { get { return battleManager; } }

    [SerializeField] private HunterScript hunter;
    public HunterScript Hunter { get { return hunter; } }

    [SerializeField] private CultistScript cultist;
    public CultistScript Cultist { get { return cultist; } }

    [SerializeField] private DemonScript demon;
    public DemonScript Demon { get { return demon; } }

    [SerializeField] private WitchScript witch;
    public WitchScript Witch { get { return witch; } }

    [SerializeField] private Menu characterSelectMenu;
    public Menu CharacterSelectMenu { get { return characterSelectMenu; } }

    [SerializeField] private EnemySelection enemySelectMenu;
    public EnemySelection EnemySelectMenu { get { return enemySelectMenu; } }

    [SerializeField] private Menu hunterMenu;
    public Menu HunterMenu { get { return hunterMenu; } }

    [SerializeField] private Menu cultistMenu;
    public Menu CultistMenu { get { return cultistMenu; } }

    [SerializeField] private Menu demonMenu;
    public Menu DemonMenu { get { return demonMenu; } }

    [SerializeField] private Menu witchMenu;
    public Menu WitchMenu { get { return witchMenu; } }
}
