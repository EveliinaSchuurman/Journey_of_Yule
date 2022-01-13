using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonItem : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemAmount;
    public TextMeshProUGUI itemName;

    public Item itemAttached;

    public void InitButton(Item item, int amount)
    {
        itemAttached = item;
        itemImage.sprite = item.itemPicture;

        if (item.itemBattleType == ItemBattleType.MISTLETOE)
        {
            itemAmount.text = "";
        }
        else if (amount == 0)
        {
            gameObject.GetComponent<Button>().interactable = false;
            itemAmount.text = amount.ToString();
        }
        else
        {
            itemAmount.text = amount.ToString();
        }

        itemName.text = item.itemName;
    }
}
