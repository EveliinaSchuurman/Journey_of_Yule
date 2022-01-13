using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinselMovement : MonoBehaviour
{
    bool mouseDown = false;
    public Tinsel tinsel;
    LineRenderer line;
    public Vector3 StartPos;

    // Start is called before the first frame update
    void Start()
    {
        tinsel = gameObject.GetComponent<Tinsel>();
        line = gameObject.GetComponent<LineRenderer>();
        StartPos = line.GetPosition(line.positionCount-1);
    }

    // Update is called once per frame
    void Update()
    {
        MoveWire();
    }

    private void OnMouseDown()
    {
        mouseDown = true;
    }

    private void OnMouseOver()
    {
        tinsel.movable = true;
    }

    private void OnMouseExit()
    {
        if (!tinsel.moving)
        {
            tinsel.movable = false;
        }
    }
    private void OnMouseUp()
    {
        mouseDown = false;
        gameObject.transform.position = tinsel.StartPos;
        line.SetPosition((line.positionCount - 1), new Vector3(StartPos.x, StartPos.y, 1));
        line.SetPosition((line.positionCount - 2), new Vector3(StartPos.x, StartPos.y, 1));
    }

    public void MoveWire()
    {
        if (mouseDown && tinsel.movable && !tinsel.connected)
        {
            line.SetPosition((line.positionCount - 1), new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 1));
            line.SetPosition((line.positionCount - 2), new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 1));
            tinsel.moving = true;
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 1));
        }
        else if (tinsel.connected)
            line.sortingOrder = -5;
        else
            tinsel.moving = false;
        
    }
}
