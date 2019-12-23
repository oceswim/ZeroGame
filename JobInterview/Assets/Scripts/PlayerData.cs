
[System.Serializable]
public class PlayerData // here we store informations of our player
{ 
    
    public int bodyIndex, faceIndex, armsIndex, legsIndex,customisationIndex;
   
    public PlayerData()
    {
        bodyIndex = faceIndex = armsIndex = legsIndex  = customisationIndex= 0;
        
    }
}
