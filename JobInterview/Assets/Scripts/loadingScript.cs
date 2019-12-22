using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingScript : MonoBehaviour
{
    private Image theImage;
    // Start is called before the first frame update
    void OnEnable()
    {
        theImage = transform.GetComponent<Image>();
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(theSlider.value);
        theImage.transform.Rotate(0, 0, 2.5f);
    }
}
