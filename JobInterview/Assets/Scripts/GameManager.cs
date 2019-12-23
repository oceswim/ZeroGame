using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //allows to create a single instance of the game manager per game
    public static GameManager instance = null;
    public bool GameIsPaused = false;
    public bool GameHasStarted = false;


    void Awake()
    {

      // if no file saved create a brand new game
        if (!File.Exists(Application.persistentDataPath + "/ThePlayerInfo.gd"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("new game");
            Debug.Log(PlayerPrefs.GetString("saved"));
            Game.current = new Game();
            
        }
        //if file found, create a new game and load saved information
        else
        {
            Debug.Log("HERE");
            Debug.Log(PlayerPrefs.GetString("saved"));

            SaveSystem.LoadPlayer();
            Game.current = new Game();
            Game.current.thePlayer.bodyIndex = SaveSystem.body;;
            Game.current.thePlayer.faceIndex = SaveSystem.face;
            Game.current.thePlayer.armsIndex = SaveSystem.arms;
            Game.current.thePlayer.legsIndex = SaveSystem.legs;

        }

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. Enforces singleton pattern: here can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }
    //before app exits, save the game instance
    public void OnApplicationQuit()
    {
        SaveSystem.SavePlayer();
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;

    }
    public void Started()
    {
        CharacterManager.startedGame = true;
        GameHasStarted = true;
    }
    //allows to keep consistency in main scene to control the character properly
    public void SwitchOffMenuBool()
    {
        CharacterManager.menuOn = false;
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
    public void LoadCustomisation()
    {
        SceneManager.LoadScene("AvatarCustomisation");
    }
}

    