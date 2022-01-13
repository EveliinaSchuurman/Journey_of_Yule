using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class minigamecontroller : MonoBehaviour
{
    private PlayerScript player;
    public int activeGame = 3; // find = 10, slide =1

    //find_and_click
    public Button find1;
    public Button find2; 
    public Button find3;
    private int objs_found = 0;
    private int objs_needed = 3;

    public GameObject youWon;

    //sliding puzzle
    [SerializeField] private Transform emptySpace;
    [SerializeField] private tileScript[] tiles;
    private Camera _camera;
    public float dist;
    private int emptySpaceIndex = 8;
    private int puzzlenum = 8;
    public GameObject slidingPuzzle;

    //jigsaw
    public GameObject jigsaw;

    //tinsel
    public int connections = 0;
    public GameObject tinsel;

    public void Awake() { 
    
        //FIND PLAYER
        _camera = Camera.main;
        if(activeGame == 0)
        {
            find1.gameObject.SetActive(true);
            find2.gameObject.SetActive(true); 
            find3.gameObject.SetActive(true);
            find1.gameObject.GetComponent<Button>().onClick.AddListener(delegate{objClick(find1);});
            find2.gameObject.GetComponent<Button>().onClick.AddListener(delegate { objClick(find2); });
            find3.gameObject.GetComponent<Button>().onClick.AddListener(delegate { objClick(find3); });
        }
        if(activeGame == 1)
        {
            slidingPuzzle.SetActive(true);
            Shuffle();
        }
        if(activeGame == 2)
        {
            jigsaw.SetActive(true);
        }
        if (activeGame == 3)
        {
            //tinsel.SetActive(true);
        }


    }

    public void objClick(Button s)
    {
        s.gameObject.SetActive(false);
        objs_found++;
        if(objs_found == objs_needed)
        {
            win();

            //add an item to inventory
            Debug.Log("you won!");
        }
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void win()
    {
        //win
        youWon.gameObject.SetActive(true);
        foreach (Transform child in youWon.transform)
        {
            if (child.name == "winText")
            {
                child.GetComponent<Text>().text = "You won! Here's an item for you";
            }
        }
        //show picture of item and 
        //add shit to inventory
    }


    void Update()
    {
        if(activeGame == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit)
                {
                    if (Vector2.Distance(emptySpace.position, hit.transform.position) < dist)
                    {
                        Vector2 lastEmptySpacePosition = emptySpace.position;
                        tileScript thisTile = hit.transform.GetComponent<tileScript>();
                        emptySpace.position = thisTile.targetPos;
                        emptySpace.position = thisTile.targetPos;
                        thisTile.targetPos = lastEmptySpacePosition;

                        int tileIndex = findIndex(thisTile);
                        tiles[emptySpaceIndex] = tiles[tileIndex];
                        tiles[tileIndex] = null;
                        emptySpaceIndex = tileIndex;
                    }
                }
            }

            int correctTiles = 0;
            foreach (var a in tiles)
            {
                if (a != null)
                {
                    if (a.inrightplace)
                        correctTiles++;
                }

            }
            if (correctTiles == tiles.Length - 1)
            {
                win();
            }
        }
        if (activeGame == 3)
        {
            if(GetConnections() == 5)
            {
                win();
            }
        }
    }

    public void SetConnections()
    {
        connections++;
    }
    public int GetConnections()
    {
        return connections;
    }

    public void Shuffle()
    {
        if(emptySpaceIndex != puzzlenum)
        {
            var tileon11LastPos = tiles[puzzlenum].targetPos;
            tiles[11].targetPos = emptySpace.position;
            emptySpace.position = tileon11LastPos;
            tiles[emptySpaceIndex] = tiles[puzzlenum];
            tiles[puzzlenum] = null;
            emptySpaceIndex = puzzlenum;
        }

        int invertion;
        do
        {
            for (int i = 0; i < puzzlenum; i++)
            {
                if (tiles[i] != null)
                {
                    var lastPos = tiles[i].targetPos;
                    int randomIndex = Random.Range(0, puzzlenum);
                    tiles[i].targetPos = tiles[randomIndex].targetPos;
                    tiles[randomIndex].targetPos = lastPos;

                    var tile = tiles[i];
                    tiles[i] = tiles[randomIndex];
                    tiles[randomIndex] = tile;

                }
            }
            Debug.Log("shuffled");
            invertion = GetInversions();
        }while(invertion%2 != 1);
    }

    public int findIndex(tileScript ts)
    {
        for(int i = 0; i <tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == ts)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tiles.Length; j++)
            {
                if (tiles[j] != null)
                {
                    if (tiles[i].number > tiles[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }
}
