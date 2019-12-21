using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData // here we store informations of our player
{ 
    
    public int bodyIndex, faceIndex, armsIndex, legsIndex;
    public PlayerData()
    {
        bodyIndex = faceIndex = armsIndex = legsIndex  = 0;
    }
}
