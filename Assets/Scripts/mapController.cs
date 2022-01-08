using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class mapController : MonoBehaviour
{
    //so this script knows when to open up new pathways
    //I might write this in a stupid way

    //script needs all the rooms
    //then I'll individually open them as they slay monsters -> needs to interact with Playerscript(keys) and battlescript(knows if monster is defeated -> win)

    public PlayerScript player;
    public GameObject[] rooms; // room 0 is room 1
    int[] openRooms = new int[9] {0,1,0,0,0,0,0,0,0}; //room'0' is obsolete

    public TMP_Text errorText;
    public Image errorBackground;

    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
    }
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < player.GetOpenRooms(); i++){
            ProgressRooms(i, false);
        }
    }

    //needs a list of open rooms to check if rooms can open
    public void ProgressRooms(int roomnumber, bool save)
    {

        //open room7? openRooms[8]
        //light up pathways and open rooms
        openRooms[roomnumber++] = 1;
        if (save == true)
            player.RoomsOpenTo(roomnumber);

        foreach(Transform child in rooms[roomnumber].transform)
        {
            if(child.name == "lights")
            {
                child.gameObject.SetActive(true);
            }
            if(child.CompareTag("training") || child.CompareTag("fight"))
            {
                child.gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void OpenTrainingRoom(int roomnumber) 
    {
        
        if (TryToOpenRoom(roomnumber) == true)//is the room open, regardless if it is training or fight
        {
            if (player.GetKeys() <= 0) //training -> does the player have keys?
            {
                StartCoroutine(ShowImage("Not enough keys!", 2));
            }
            //check if player has enough keys and substract

            else
            {
                player.AmountOfKeys(false);
                SceneManager.LoadScene("training");
            }
            //playerScript takes in room param so the rooms know what to load
        }
        else
        {
            //shows errormsg
            StartCoroutine(ShowImage("Room is not open yet!", 1));
        }
    }
    public void OpenFightRoom(int roomnumber)
    {

        if (TryToOpenRoom(roomnumber) == true)//is the room open, regardless if it is training or fight
        {

            //has to remember the room theyre in (or I have to think this again

            SceneManager.LoadScene("fight");
            //playerScript takes in room param so the rooms know what to load
        }
        else
        {
            //shows errormsg
            StartCoroutine(ShowImage("Room is not open yet!", 1));
        }
    }

    private bool TryToOpenRoom(int roomnum)
    {
        if (openRooms[roomnum] == 1)
        {
            return true;
        }
        else return false;
    }

    IEnumerator ShowMessage(string msg, float delay)
    {

        for (float i = 1; i >= 0; i -= Time.deltaTime / delay)
        {
            // set color with i as alpha
            errorText.gameObject.GetComponent<TMP_Text>().color = new Color(0, 0, 0, i);
            yield return null;
            Debug.Log("fading");
        }
        errorText.gameObject.SetActive(false);
        errorBackground.gameObject.SetActive(false);
    }
    IEnumerator ShowImage(string msg, float delay)
    {
        errorText.text = msg;
        errorText.gameObject.SetActive(true);
        errorBackground.gameObject.SetActive(true);
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime*delay)
        {
            yield return null;
            StartCoroutine(ShowMessage(msg, delay));
            Debug.Log("showing");
        }

    }
}
