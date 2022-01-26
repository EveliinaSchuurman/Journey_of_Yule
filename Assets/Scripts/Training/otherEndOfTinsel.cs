using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherEndOfTinsel : MonoBehaviour
{
    minigamecontroller minigamecontroller;
    public bool connected = false;
    public int numCon;

    private void Start()
    {
        minigamecontroller = FindObjectOfType<minigamecontroller>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Tinsel>())
        {
            Tinsel tinsel = collision.GetComponent<Tinsel>();
            if(tinsel.num == numCon)
            {
                tinsel.connected = true;
                connected = true;
                minigamecontroller.SetConnections();
                foreach(Transform child in collision.gameObject.transform)
                {
                    if (child.name == "craftedTinsel")
                        child.gameObject.SetActive(true);

                }
            }
        }
    }

}
