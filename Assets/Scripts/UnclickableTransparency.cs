using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnclickableTransparency : MonoBehaviour
{

    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 1.0f;
    }
}
