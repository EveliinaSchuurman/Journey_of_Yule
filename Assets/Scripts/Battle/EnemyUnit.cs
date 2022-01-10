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

    private string enemyName;
    private float maxHealth;
    private float currentHealth;
    private float damage;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetUpEnemy(Enemy enemy)
    {
        hitButton.onClick.AddListener(TakeDamage);
        hitButton.enabled = false;

        enemyPicture.sprite = enemy.picture;
        enemyName = enemy.enemyName;
        maxHealth = enemy.maxHP;
        currentHealth = enemy.maxHP;
        damage = enemy.damage;
    }

    public void EnableHitBox()
    {
        hitButton.enabled = true;
    }

    public void DisableHitBox()
    {
        hitButton.enabled = false;
    }

    private void TakeDamage()
    {
        Debug.Log(enemyName + " took damage");
    }

}

