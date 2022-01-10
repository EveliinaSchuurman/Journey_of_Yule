using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Image playerHPFilled;
    public TextMeshProUGUI HPText;

    public GameObject itemGrid;

    public GameObject itemButtonPrefab;

    public void ShowItems()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject newItemButton = Instantiate(itemButtonPrefab, itemButtonPrefab.transform.position, itemButtonPrefab.transform.rotation);
            newItemButton.transform.SetParent(itemGrid.transform, false);
        }
    }

    public void HideItems()
    {
        foreach (Transform child in itemGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
