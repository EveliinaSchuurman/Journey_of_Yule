using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Image playerHPFilled;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI bottomText;

    public GameObject itemGrid;

    public GameObject itemButtonPrefab;

    public List<Item> itemsList = new List<Item>();


    public void ShowItems(Inventory inv)
    {
        foreach (Item item in itemsList)
        {
            GameObject newItemButton = Instantiate(itemButtonPrefab, itemButtonPrefab.transform.position, itemButtonPrefab.transform.rotation);
            newItemButton.transform.SetParent(itemGrid.transform, false);
            newItemButton.GetComponent<ButtonItem>().InitButton(item, inv.GetNumberOfItem(item.itemBattleType));
        }
    }



    public void HideItems()
    {
        foreach (Transform child in itemGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void ShowBottomText(string textShown)
    {
        bottomText.text = textShown;
        bottomText.gameObject.SetActive(true);
    }

    public void HideBottomText()
    {
        bottomText.gameObject.SetActive(false);
    }
}
