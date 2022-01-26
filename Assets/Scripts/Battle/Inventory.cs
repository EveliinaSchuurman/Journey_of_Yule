using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int keys;
    private int coins;

    public List<Item> inventoryList = new List<Item>();
    public List<BattleCompanion> inventoryCompanionList = new List<BattleCompanion>();

    public int GetNumberOfItem(ItemBattleType itemType)
    {
        int count = 0;

        foreach (Item i in inventoryList) 
        {
            if (i.itemBattleType == itemType)
                count++;
        }

        return count;
    }

    public bool RemoveItem(Item item)
    {
        if(item.itemBattleType == ItemBattleType.MISTLETOE)
            return false;
        else
            return inventoryList.Remove(item);
    }

    public int GetKeys()
    {
        return keys;
    }

    public int GetCoins()
    {
        return coins;
    }

    public bool AddKeys(int amount)
    {
        keys += amount;

        if (keys < 0)
        {
            keys = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool AddCoins (int amount)
    {
        coins += amount;

        if (coins < 0)
        {
            coins = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool AddCompanion (Companion companion)
    {
        foreach (BattleCompanion bc in inventoryCompanionList)
        {
            if (bc._coObject == companion)
            {
                // Already have this companion
                return false;
            }
        }

        inventoryCompanionList.Add(new BattleCompanion(companion));
        return true;
    }

    public bool RemoveCompanion(BattleCompanion bCompanion)
    {
        return inventoryCompanionList.Remove(bCompanion);
    }
}

public class BattleCompanion
{
    public Companion _coObject;
    public int _coCurrentHearts;

    public BattleCompanion (Companion companion)
    {
        _coObject = companion;
        _coCurrentHearts = companion.coMaxHearts;
    }
}
