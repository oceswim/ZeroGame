using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool GameIsPaused = false;
    public bool GameHasStarted = false;

    //public static int score, health, energy;
    public RectTransform recipes, customisation;

    void Awake()
    {

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
    void Update()
    {

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
    //Call this to add the passed in Enemy to the List of Enemy objects.
   
    public void QuitGame()
    {

    }
    public void FirstLoad()
    {



    }

    public void Continue()
    {
    }
    public void ResetHealth()
    {

    }

    public void TryAgain()
    {


    }
    public void Personnalisation()
    {
        Pause();
        customisation.gameObject.SetActive(true);
    }
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

}

    