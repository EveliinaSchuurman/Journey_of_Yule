using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    // Player UI
    public Image playerHPFilled;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI bottomText;
    public GameObject itemGrid;

    public GameObject itemButtonPrefab;

    public List<Item> itemsList = new List<Item>();

    // TEST PLAYER
    private Inventory inv = new Inventory();

    void Start()
    {
        MakeTestInventory();

        InitializeBattle();
    }


    // Player turn
    private void PlayerTurn()
    {
        battleState = BattleState.PLAYER_TURN;

        ShowItems(inv);
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

        // Randomize small enemy amount and type
        int smallEnemyAmount = Random.Range(0, smallEnemyUnit.Count + 1);
        Enemy smallEnemyType;
        smallEnemyType = smallEnemies[Random.Range(0, smallEnemies.Count)];

        for(int i = 0; i < smallEnemyUnit.Count; i++)
        {
            if (i < smallEnemyAmount)
            {
                smallEnemyUnit[i].SetUpEnemy(smallEnemyType);
                enemyUnitsInBattle.Add(smallEnemyUnit[i]);
            }  
            else
            {
                smallEnemyUnit[i].gameObject.SetActive(false);
            }
        }
    }

    // Display player items
    private void ShowItems(Inventory inv)
    {
        int count = 0;

        foreach (Item item in itemsList)
        {
            GameObject newItemButton = Instantiate(itemButtonPrefab, itemButtonPrefab.transform.position, itemButtonPrefab.transform.rotation);
            newItemButton.transform.SetParent(itemGrid.transform, false);
            newItemButton.GetComponent<ButtonItem>().InitButton(item, inv.GetNumberOfItem(item.itemBattleType));

            int tmp = count;

            newItemButton.GetComponent<Button>().onClick.AddListener(delegate { ItemButtonOnClick(tmp); });

            count++;
        }
    }

    // OnClick function for items
    private void ItemButtonOnClick(int i)
    {
        HideItems();
        inv.RemoveItem(itemsList[i]);

        ShowBottomText("Using item: " + itemsList[i].itemName + "\nSelect a target");
    }

    private void HideItems()
    {
        foreach (Transform child in itemGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void ShowBottomText(string textShown)
    {
        bottomText.text = textShown;
        bottomText.gameObject.SetActive(true);
    }

    private void HideBottomText()
    {
        bottomText.gameObject.SetActive(false);
    }

    private void InitializeBattle()
    {
        SpawnEnemies();

        PlayerTurn();
    }

    // TEST METHODS
    private void MakeTestInventory()
    {
        inv.inventoryList.Add(itemsList[0]);
        inv.inventoryList.Add(itemsList[1]);
        inv.inventoryList.Add(itemsList[2]);
        inv.inventoryList.Add(itemsList[3]);
        inv.inventoryList.Add(itemsList[4]);
        inv.inventoryList.Add(itemsList[2]);
        inv.inventoryList.Add(itemsList[1]);
        inv.inventoryList.Add(itemsList[2]);
        inv.inventoryList.Add(itemsList[3]);
        inv.inventoryList.Add(itemsList[3]);
        inv.inventoryList.Add(itemsList[3]);
        inv.inventoryList.Add(itemsList[2]);
        inv.inventoryList.Add(itemsList[5]);
    }
}
