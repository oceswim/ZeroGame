
using UnityEngine;

public class QuitApp : MonoBehaviour
{
    public GameObject QuitQuestion;
   
    // check if user wants to exit game
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            GameManager.instance.Pause();
            QuitQuestion.SetActive(true);
        }
    }
}
