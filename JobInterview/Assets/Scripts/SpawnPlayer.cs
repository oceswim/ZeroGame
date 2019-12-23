using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject thePlayer;

    //Activates our player when game starts
    void Start()
    {
        thePlayer.SetActive(true);
    }

}
