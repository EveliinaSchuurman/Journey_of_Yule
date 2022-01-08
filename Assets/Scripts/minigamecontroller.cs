using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class minigamecontroller : MonoBehaviour
{
    private PlayerScript player;
    private int activeGame = 1; // find = 10, slide =1

    //find_and_click
    public Button find1;
    public Button find2; 
    public Button find3;
    private int objs_found = 0;
    private int objs_needed = 3;

    public Text youWonText;

    //sliding puzzle
    [SerializeField] private GameObject emptySpace;
    [SerializeField] private tileScript[] tiles;
    private Camera _camera;
    public int dist;
    public Button puzzle_1;
    public Button puzzle_2;
    public Button puzzle_3;
    public Button puzzle_4;
    public Button puzzle_5;
    public Button puzzle_6;
    public Button puzzle_7;
    public Button puzzle_8;
    public Button puzzle_9;
    public Button puzzle_10;
    public Button puzzle_11;

    public void Start()
    {
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
            puzzle_1.gameObject.SetActive(true);
            puzzle_2.gameObject.SetActive(true);
            puzzle_3.gameObject.SetActive(true);
            puzzle_4.gameObject.SetActive(true);
            puzzle_5.gameObject.SetActive(true);
            puzzle_6.gameObject.SetActive(true);
            puzzle_7.gameObject.SetActive(true);
            puzzle_8.gameObject.SetActive(true);
            puzzle_9.gameObject.SetActive(true);
            puzzle_10.gameObject.SetActive(true);
            puzzle_11.gameObject.SetActive(true);
            puzzle_1.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_1); });
            puzzle_2.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_2); });
            puzzle_3.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_3); });
            puzzle_4.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_4); });
            puzzle_5.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_5); });
            puzzle_6.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_6); });
            puzzle_7.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_7); });
            puzzle_8.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_8); });
            puzzle_9.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_9); });
            puzzle_10.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_10); });
            puzzle_11.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SlidingPuzzleClick(puzzle_11); });

        }


    }

    public void objClick(Button s)
    {
        s.gameObject.SetActive(false);
        objs_found++;
        if(objs_found == objs_needed)
        {
            youWonText.text = "You won! Here's an item for you";

            //add an item to inventory
            Debug.Log("you won!");
        }
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene("Map");
    }


    private void SlidingPuzzleClick(Button thisBtn)
    {
        if(Vector2.Distance(thisBtn.transform.position, emptySpace.transform.position) < dist)
        {
            Vector2 lastEmptySpacePosition = emptySpace.transform.position;
            tileScript thisTile = thisBtn.transform.GetComponent<tileScript>();
            emptySpace.transform.position = thisTile.targetPos;
            thisTile.targetPos = lastEmptySpacePosition;
        }
    }

}
