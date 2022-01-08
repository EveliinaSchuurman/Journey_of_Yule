using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class minigamecontroller : MonoBehaviour
{
    private PlayerScript player;
    private int activeGame;

    //find_and_click
    public Button find1;
    public Button find2; 
    public Button find3;
    private int objs_found = 0;
    private int objs_needed = 3;

    public Text youWonText;

    //sliding puzzle
    [SerializeField] private GameObject emptySpace;
    private Camera _camera;

    public void Start()
    {
        _camera = Camera.main;
        find1.gameObject.SetActive(true);
        find2.gameObject.SetActive(true); 
        find3.gameObject.SetActive(true);
        find1.gameObject.GetComponent<Button>().onClick.AddListener(delegate{objClick(find1);});
        find2.gameObject.GetComponent<Button>().onClick.AddListener(delegate { objClick(find2); });
        find3.gameObject.GetComponent<Button>().onClick.AddListener(delegate { objClick(find3); });


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

    private void Update()
    {
        if
    }

}
