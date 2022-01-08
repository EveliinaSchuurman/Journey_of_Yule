using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileScript : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 correctPos;

    // Start is called before the first frame update
    void Awake()
    {
        targetPos = transform.position;
        correctPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
    }
}
