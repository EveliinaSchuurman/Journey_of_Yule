using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST}

public class BattleController : MonoBehaviour
{
    [SerializeField]
    private BattleState battleState;

    // Enemy
    public EnemyUnit bigEnemyUnit;
    public List<EnemyUnit> smallEnemyUnit;

    private List<EnemyUnit> enemyUnitsInBattle = new List<EnemyUnit>();

    public Enemy bossEnemy;
    public List<Enemy> bigEnemies = new List<Enemy>();
    public List<Enemy> smallEnemies = new List<Enemy>();

    // Player
    public PlayerUI playerHUD;

    void Start()
    {
        InitializeBattle();
    }

    private void PlayerTurn()
    {
        battleState = BattleState.PLAYER_TURN;

        playerHUD.ShowItems();
    }

    private void SpawnEnemies()
    {
        bool isBossRoom = false;

        if (isBossRoom) {
            // Boss Pumpkin
            bigEnemyUnit.SetUpEnemy(bossEnemy);
        }
        else {
            // Normal big enemy
            bigEnemyUnit.SetUpEnemy(bigEnemies[Random.Range(0, bigEnemies.Count)]);
        }

        enemyUnitsInBattle.Add(bigEnemyUnit);
        bigEnemyUnit.gameObject.SetActive(true);

        // Randomize small enemy amount and type
        int smallEnemyAmount = Random.Range(0, smallEnemyUnit.Count + 1);
        Enemy smallEnemyType;
        smallEnemyType = smallEnemies[Random.Range(0, smallEnemies.Count)];

        for(int i = 0; i < smallEnemyAmount; i++)
        {
            smallEnemyUnit[i].SetUpEnemy(smallEnemyType);
            enemyUnitsInBattle.Add(smallEnemyUnit[i]);
            smallEnemyUnit[i].gameObject.SetActive(true);
        }

    }

    public void InitializeBattle()
    {
        SpawnEnemies();

        PlayerTurn();
    }
}
