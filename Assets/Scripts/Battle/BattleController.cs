using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum BattleState { START, PLAYER_TURN, COMPANION_TURN, ENEMY_TURN, WON, LOST}

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
    public Image playerAvatar;
    public Image playerHPFilled;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI bottomText;
    public GameObject itemGrid;

    public GameObject itemButtonPrefab;
    public Sprite playerSprite;

    public List<Item> itemsList = new List<Item>();

    // Player
    private Item selectedItem;

    public List<Companion> companionsList = new List<Companion>();

    [SerializeField]
    private float playerMaxHP;
    [SerializeField]
    private float playerCurrentHP;

    // TEST PLAYER
    private Inventory inv = new Inventory();

    void Start()
    {
        MakeTestInventory();

        InitializeBattle();
    }


    // Select actions based on whose turn it is
    private void TurnSelector(BattleState bState)
    {
        battleState = bState;

        switch (battleState) {

            case BattleState.PLAYER_TURN:
                // Player
                ShowItems(inv);
                break;

            case BattleState.COMPANION_TURN:
                // Companion
                foreach (BattleCompanion co in inv.companionList)
                {
                    playerAvatar.sprite = co._coObject.coPicture;
                    CompanionTurn(co);
                }
                playerAvatar.sprite = playerSprite;
                TurnSelector(BattleState.ENEMY_TURN);
                break;

            case BattleState.ENEMY_TURN:
                // Enemy
                Debug.Log("Enemy turn");
                break;

            default:
                Debug.Log("No battle state found");
                break;
        }
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
        selectedItem = itemsList[i];

        ShowBottomText("Using item: " + itemsList[i].itemName + "\nSelect a target");

        foreach (EnemyUnit enemy in enemyUnitsInBattle)
        {
            enemy.EnableHitBox();
        }
    }

    public void UseItem(EnemyUnit target)
    {
        foreach (EnemyUnit enemy in enemyUnitsInBattle)
        {
            enemy.DisableHitBox();
        }

        HideBottomText();

        // Check if alive
        if (!target.TakeDamage(selectedItem.itemDamage))
        {
            enemyUnitsInBattle.Remove(target);
            target.gameObject.SetActive(false);

            if (CheckIfEnd())
                EndBattle();
            else
                TurnSelector(BattleState.COMPANION_TURN);
        } 
        else
        {
            TurnSelector(BattleState.COMPANION_TURN);
        }
    }

    private void CompanionTurn(BattleCompanion companion)
    {
        Debug.Log("Companion turn: " + companion._coObject.coName);

        switch (companion._coObject.coType)
        {
            case CompanionType.SNOWMAN:
                // Blizzard
                ShowBottomText(companion._coObject.coName + " used Blizzard!");

                for (int i = enemyUnitsInBattle.Count - 1; i >= 0; i--)
                {
                    if (!enemyUnitsInBattle[i].TakeDamage(companion._coObject.coPower))
                    {
                        enemyUnitsInBattle[i].gameObject.SetActive(false);
                        enemyUnitsInBattle.RemoveAt(i);
                    }
                }
                break;

            case CompanionType.ELF:
                // Healing Starlight
                ShowBottomText(companion._coObject.coName + " used Healing Starlight!");
                playerCurrentHP = (playerCurrentHP + companion._coObject.coPower) > playerMaxHP ? playerMaxHP : (playerCurrentHP + companion._coObject.coPower);
                break;

            case CompanionType.GINGERBREAD_MAN:
                // Rock Dough
                ShowBottomText(companion._coObject.coName + " used Rock Dough!");

                break;
        }

        HideBottomText();
    }

    private bool CheckIfEnd()
    {
        // Enemy won
        if (playerCurrentHP <= 0)
        {
            Debug.Log("You ded lol");
            battleState = BattleState.LOST;
            return true;
        } 
        // Player won
        else if (enemyUnitsInBattle.Count <= 0) {
            Debug.Log("You won! WOW");
            battleState = BattleState.WON;
            return true;
        } 
        // Battle continues
        else
        {
            return false;
        }
    }

    private void EndBattle()
    {
        Debug.Log("Battle end, result: " + battleState.ToString());
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

        TurnSelector(BattleState.PLAYER_TURN);
    }

    // TEST METHODS
    private void MakeTestInventory()
    {
        inv.inventoryList.Add(itemsList[1]);
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

        inv.AddCompanion(companionsList[0]);
        inv.AddCompanion(companionsList[1]);
        inv.AddCompanion(companionsList[2]);
    }
}
