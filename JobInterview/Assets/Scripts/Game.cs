[System.Serializable]
public class Game
{
    public static Game current;
    public PlayerData thePlayer;
    //allows to save the needed data by creating new instances of a game and then saving/loading it
    public Game()
    {
        thePlayer = new PlayerData();
    }

}