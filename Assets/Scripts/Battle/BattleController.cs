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
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI bottomText;
    public GameObject itemGrid;
    public GameObject playerInfo;

    public GameObject itemButtonPrefab;
    public Sprite playerSprite;

    public List<Item> itemsList = new List<Item>();

    // Companion UI
    public GameObject companionInfo;
    public GameObject companionHeartPrefab;

    // Player
    private Item selectedItem;

    public List<Companion> companionsList = new List<Companion>();

    [SerializeField]
    private float playerMaxHP;
    [SerializeField]
    private float playerCurrentHP;

    // TEST PLAYER
    private Inventory inv = new Inventory();

    // Timers
    private WaitForSeconds shortWait = new WaitForSeconds(0.75f);
    private WaitForSeconds mediumWait = new WaitForSeconds(1.2f);

    void Start()
    {
        MakeTestPlayer();

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
                StartCoroutine(CompanionTurn());
                break;

            case BattleState.ENEMY_TURN:
                // Enemy
                StartCoroutine(EnemyTurn());
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
        if (!target.TakeDamage(selectedItem))
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

    IEnumerator CompanionTurn()
    {
        foreach (BattleCompanion companion in inv.inventoryCompanionList)
        {
            if (enemyUnitsInBattle.Count == 0)
            {
                break;
            }

            playerAvatar.sprite = companion._coObject.coPicture;

            switch (companion._coObject.coType)
            {
                case CompanionType.SNOWMAN:
                    //Blizzard
                    ShowBottomText(companion._coObject.coName + " used Blizzard!");

                    yield return mediumWait;

                    for (int i = enemyUnitsInBattle.Count - 1; i >= 0; i--)
                    {
                        if (!enemyUnitsInBattle[i].TakeDirectDamage(companion._coObject.coPower))
                        {
                            enemyUnitsInBattle[i].gameObject.SetActive(false);
                            enemyUnitsInBattle.RemoveAt(i);
                        }
                    }
                    break;

                case CompanionType.ELF:
                    // Healing Starlight
                    ShowBottomText(companion._coObject.coName + " used Healing Starlight!");

                    yield return shortWait;

                    PlayerUpdateHP(companion._coObject.coPower);

                    yield return shortWait;

                    break;

                case CompanionType.GINGERBREAD_MAN:
                    // Rock Dough
                    ShowBottomText(companion._coObject.coName + " used Rock Dough!");

                    yield return mediumWait;

                    EnemyUnit targetUnit = enemyUnitsInBattle[Random.Range(0, enemyUnitsInBattle.Count)];
                    if (!targetUnit.TakeDirectDamage(companion._coObject.coPower))
                    {
                        targetUnit.gameObject.SetActive(false);
                        enemyUnitsInBattle.Remove(targetUnit);
                    }
                    break;
            }

            HideBottomText();
        }

        playerAvatar.sprite = playerSprite;

        if (CheckIfEnd())
            EndBattle();
        else
            TurnSelector(BattleState.ENEMY_TURN);
    }

    IEnumerator EnemyTurn()
    {
        foreach (EnemyUnit enemy in enemyUnitsInBattle)
        {
            // Chooses target at random
            int target = Random.Range(-1, inv.inventoryCompanionList.Count);

            // Attack player if -1, otherwise choose from inv.inventoryCompanionList
            if (target == -1)
            {
                DisplayPlayerInfo();

                // Check if player survives damage taken
                if (!PlayerUpdateHP(-enemy.enemyObject.damage))
                {
                    yield return shortWait;
                    break;
                }

                yield return shortWait;
            }
            else
            {
                DisplayCompanionInfo(inv.inventoryCompanionList[target]);

                yield return shortWait;

                inv.inventoryCompanionList[target]._coCurrentHearts -= 1;
                DisplayCompanionInfo(inv.inventoryCompanionList[target]);

                if (inv.inventoryCompanionList[target]._coCurrentHearts == 0)
                {
                    ShowBottomText(inv.inventoryCompanionList[target]._coObject.coName + " died");

                    inv.inventoryCompanionList.RemoveAt(target);
                }

                yield return mediumWait;

                HideBottomText();
                DisplayPlayerInfo();
            }
        }

        if (CheckIfEnd())
            EndBattle();
        else
            TurnSelector(BattleState.PLAYER_TURN);
    }

    private bool PlayerUpdateHP(float updateAmount)
    {
        playerCurrentHP += updateAmount;

        // Current HP < 0, player dead
        if (playerCurrentHP <= 0)
        {
            playerCurrentHP = 0.0f;
            playerHPText.text = "0 HP";
            playerHPFilled.fillAmount = 0.0f;
            return false;
        }
        // Current HP > max HP, reset to max HP
        else if (playerCurrentHP > playerMaxHP)
        {
            playerCurrentHP = playerMaxHP;
            playerHPText.text = Mathf.Round(playerCurrentHP).ToString() + " HP";
            playerHPFilled.fillAmount = 1.0f;
            return true;
        }
        // Current HP between 0 and max
        else
        {
            playerHPText.text = Mathf.Round(playerCurrentHP).ToString() + " HP";
            playerHPFilled.fillAmount = (playerCurrentHP / playerMaxHP);
            return true;
        }
    }

    private bool CheckIfEnd()
    {
        // Enemy won
        if (playerCurrentHP <= 0)
        {
            battleState = BattleState.LOST;
            return true;
        } 
        // Player won
        else if (enemyUnitsInBattle.Count <= 0) {
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
        HideItems();
        ShowBottomText("Battle result: " + battleState.ToString());
    }

    private void HideItems()
    {
        foreach (Transform child in itemGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void DisplayCompanionInfo(BattleCompanion companion)
    {
        playerInfo.SetActive(false);
        playerAvatar.sprite = companion._coObject.coPicture;
        HideCompanionHearts();

        for (int i = 1; i <= companion._coObject.coMaxHearts; i++)
        {
            GameObject newHeart = Instantiate(companionHeartPrefab, companionHeartPrefab.transform.position, companionHeartPrefab.transform.rotation);
            newHeart.transform.SetParent(companionInfo.transform, false);

            if (i > companion._coCurrentHearts)
            {
                newHeart.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void HideCompanionHearts()
    {
        foreach (Transform child in companionInfo.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void DisplayPlayerInfo()
    {
        playerAvatar.sprite = playerSprite;
        HideCompanionHearts();
        playerInfo.SetActive(true);
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
    private void MakeTestPlayer()
    {
        playerMaxHP = 100;
        playerCurrentHP = 10;

        playerHPFilled.fillAmount = (playerCurrentHP / playerMaxHP);
        playerHPText.text = Mathf.Round(playerCurrentHP).ToString() + " HP";

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
