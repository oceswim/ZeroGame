using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manageRecipeSearch : MonoBehaviour
{
    public Toggle ingredientsToggle, DishToggle;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        ingredientsToggle.isOn=false;
        DishToggle.isOn = false;
    }
}
