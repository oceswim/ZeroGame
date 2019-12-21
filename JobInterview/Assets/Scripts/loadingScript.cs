using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingScript : MonoBehaviour
{
    private Slider theSlider;
    // Start is called before the first frame update
    void OnEnable()
    {
        theSlider = transform.GetComponent<Slider>();
        theSlider.value = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(theSlider.value);
        theSlider.value += (Time.deltaTime/10f)*100f;
    }
}
