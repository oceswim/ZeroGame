using UnityEngine;
using UnityEngine.UI;

public class ManageRecipeSearch : MonoBehaviour
{
    public Toggle ingredientsToggle, DishToggle;
    
   //resets the toggles on the reciper search page 
    void OnEnable()
    {
        ingredientsToggle.isOn=false;
        DishToggle.isOn = false;
    }
}
