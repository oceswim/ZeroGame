using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool GameIsPaused = false;
    public bool GameHasStarted = false;


    public RectTransform recipes;

    void Awake()
    {

        if (!File.Exists(Application.persistentDataPath + "/ThePlayerInfo.gd"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("new game");
            Debug.Log(PlayerPrefs.GetString("saved"));
            Game.current = new Game();
            
        }
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

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Adds new instance of jewel

    }

    //Update is called every frame.

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
    //Call this to add the passed in Enemy to the List of Enemy objects.

  
    public void RecipeResearch()
    {
        Pause();
        recipes.gameObject.SetActive(true);
    }
    public void Started()
    {
        CharacterMovement.startedGame = true;
        GameHasStarted = true;
    }

    public void loadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
    public void loadCustomisation()
    {
        SceneManager.LoadScene("AvatarCustomisation");
    }
}

    