using UnityEngine;
using UnityEngine.UI;


/*
 * Animates the loading screen by rotating the sprite
 */
public class LoadingScript : MonoBehaviour
{
    private Image theImage;
    void OnEnable()
    {
        theImage = transform.GetComponent<Image>();
        
    }

    void Update()
    {
        theImage.transform.Rotate(0, 0, 2.5f);
    }
}
