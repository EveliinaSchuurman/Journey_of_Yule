using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private int keys = 0;
    private int currentRoomNumber = 0;
    private int roomsOpeneduntil = 0;

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
    public void RoomsOpenTo(int roomNum)
    {
        roomsOpeneduntil = roomNum;
    }

    public int GetOpenRooms()
    {
        return roomsOpeneduntil;
    }
}
