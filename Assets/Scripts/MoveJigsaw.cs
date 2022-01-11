using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MoveJigsaw : MonoBehaviour
{
    private Vector3 rightPos;
    public bool inRightPos;
    public bool Selected;
    public bool counted = false;
    private Jigsaw ji;
    void Start()
    {
        ji = FindObjectOfType<Jigsaw>();
        rightPos = transform.position;
        transform.position = new Vector3(Random.Range(-1.91f, -1f), Random.Range(3.79f, 3f));
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(rightPos, transform.position) < 0.4f)
        {
            if (!Selected)
            {
                if(inRightPos == false)
                {
                    transform.position = rightPos;
                    inRightPos = true;
                    ji.AddRights();
                    GetComponent<SortingGroup>().sortingOrder = -1;
                }
            }
            if (inRightPos == true)
            {
                GetComponent<SortingGroup>().sortingOrder = -1;
            }


        }
    }
}
