using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySize { BOSS, BIG, SMALL }
public enum EnemyType { BIG_PUMPKIN, PUMPKIN, SPIDER, WITCH, SKELETON }

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    public EnemySize enemySize;
    public EnemyType enemyType;

    public Sprite picture;

    public float maxHP;
    public float damage;
    public string enemyName;

    public ItemBattleType resistance;
    public ItemBattleType weakness;
}
