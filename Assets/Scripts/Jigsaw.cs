using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Jigsaw : MonoBehaviour
{
    public GameObject SelectedPiece;
    public int orderIL = 1;
    private minigamecontroller _mini;
    public int rightPieces = 0;
    public Sprite[] sprites;
    void Start()
    {
        int rand = Random.Range(0, (sprites.Length -1));
        _mini = FindObjectOfType<minigamecontroller>();
        if (_mini == null)
            Debug.Log("not found!");
        for(int i = 0; i < 15; i++)
        {
            GameObject.Find("puzzle(" + i + ")").transform.GetComponent<SpriteRenderer>().sprite = sprites[rand];
        }
    }

    void Update()
    {
        if (_mini.activeGame == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.transform.CompareTag("Puzzle"))
                {
                Debug.Log("hit");
                    if (hit.transform.GetComponent<MoveJigsaw>().inRightPos == false)
                    {
                        SelectedPiece = hit.transform.gameObject;
                        SelectedPiece.GetComponent<MoveJigsaw>().Selected = true;
                        SelectedPiece.GetComponent<SortingGroup>().sortingOrder = orderIL;
                        orderIL++;
                    }

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if(SelectedPiece != null)
            {
                SelectedPiece.GetComponent<MoveJigsaw>().Selected = false;
                SelectedPiece = null;
            }
            }
            if (SelectedPiece != null)
            {
                Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SelectedPiece.transform.position = new Vector3(MousePoint.x, MousePoint.y, 0);
            }


        }
    }
    public void AddRights()
    {
        rightPieces++;
        if (rightPieces == 15)
        {
            _mini.win();
        }
           
        
    }
}
