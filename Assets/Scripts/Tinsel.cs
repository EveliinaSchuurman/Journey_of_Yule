using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tinsel : MonoBehaviour
{
    public bool movable = false;
    public bool moving = false;
    public Vector3 StartPos;
    public int num;
    public bool connected = false;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
