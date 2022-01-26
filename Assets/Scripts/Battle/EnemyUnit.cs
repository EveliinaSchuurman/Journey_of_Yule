using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{
    public Image enemyPicture;
    public Image HPBarBG;
    public Image HPFilled;

    public Button hitButton;

    public Enemy enemyObject;
    private string enemyName;
    private float maxHealth;
    private float currentHealth;

    private BattleController battleController;

    private void Start()
    {
        if (battleController == null)
            battleController = GameObject.FindWithTag("Battle System").GetComponent<BattleController>();
    }

    public void SetUpEnemy(Enemy enemy)
    {
        hitButton.onClick.AddListener(TakeHit);
        hitButton.enabled = false;

        enemyObject = enemy;
        enemyPicture.sprite = enemy.picture;
        enemyName = enemy.enemyName;
        maxHealth = enemy.maxHP;
        currentHealth = enemy.maxHP;
    }

    public void EnableHitBox()
    {
        hitButton.enabled = true;
    }

    public void DisableHitBox()
    {
        hitButton.enabled = false;
    }

    private void TakeHit()
    {
        battleController.UseItem(this);
    }

    // Deal damage based on an item
    public bool TakeDamage(Item item)
    {
        float damageDealt;

        // Check weakness and resistance
        if(enemyObject.weakness == item.itemBattleType)
        {
            damageDealt = item.itemDamage * 2;
        } 
        else if (enemyObject.resistance == item.itemBattleType)
        {
            damageDealt = item.itemDamage / 2;
        }
        else
        {
            damageDealt = item.itemDamage;
        }

        currentHealth -= damageDealt;

        if (currentHealth <= 0)
        {
            HPFilled.fillAmount = (currentHealth / maxHealth);
            return false;
        } 
        else
        {
            HPFilled.fillAmount = (currentHealth / maxHealth);
            return true;
        }
    }

    // Deal damage based on a float number
    public bool TakeDirectDamage(float damageDealt)
    {
        currentHealth -= damageDealt;

        if (currentHealth <= 0)
        {
            HPFilled.fillAmount = (currentHealth / maxHealth);
            return false;
        }
        else
        {
            HPFilled.fillAmount = (currentHealth / maxHealth);
            return true;
        }
    }

}

