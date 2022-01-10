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

    public void InitButton(Sprite sprite, int amount, string name)
    {
        itemImage.sprite = sprite;
        itemAmount.text = amount.ToString();
        itemName.text = name;
    }
}
