using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private int keys = 1;
    private int currentRoomNumber = 0;
    private int _openRooms = 0;

    private void Awake()
    {
        //add keys if first run
        //load playerprefs
    }
    public void AmountOfKeys(bool posneg)
    {
        if (posneg == true)
        {
            keys++;
        }
        else keys--;
    }
    public int GetKeys()
    {
        return keys;
    }

    public void SetRoom(int roomNum)
    {
        currentRoomNumber = roomNum;
    }

    public int GetRoom()
    {
        return currentRoomNumber;
    }
    public void OpenRoom()
    {
        _openRooms++;
    }

    public int GetOpenRooms()
    {
        //this can be an int because all doors can only be opened in order
        return _openRooms;
    }

}
