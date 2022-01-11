using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemBattleType { MISTLETOE, CANDYCANE, LIGHTS, BELL, GINGERBREAD, GIFT }

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public ItemBattleType itemBattleType;
    public Sprite itemPicture;
    public string itemName;
    public float itemDamage;
}
