[System.Serializable]
public class Game
{
    public static Game current;
    public PlayerData thePlayer;
    // Start is called before the first frame update
    public Game()
    {
        thePlayer = new PlayerData();
    }

}