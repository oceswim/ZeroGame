using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class handleInputs : MonoBehaviour
{
    public TMP_InputField ingredients, dishes;
    private bool dishOn, ingredientsOn;
    private string[] ingredientsNames;
    private string dishInput;
    public GameObject errorInput, errorIngredient, errorDish;
    // Start is called before the first frame update
    void OnEnable()
    {
        ingredientsNames = new string[] { };
        dishInput = "";
        dishOn = ingredientsOn = false;
    }
    private void OnDisable()
    {
        ingredients.text = dishes.text = "";
    }

    // Update is called once per frame
    public void IngredientsInput()
    {
        int errorCount = 0;
        if (ingredients.text.Length > 0)
        {
            if (ingredients.text.Contains(","))
            {
          
                ingredientsNames = SplitInput(ingredients.text);
               
            }
            else if (ingredients.text.Contains(" "))
            {
                errorCount++;
                errorIngredient.SetActive(true);
            }
            else
            {
     
                string[] singleIngredient = { ingredients.text };

                ingredientsNames = singleIngredient;
            }
        }
        else
        {
            errorCount++;
            errorIngredient.SetActive(true);
        }
        if(errorCount==0)
        {
            SetBoolIngredient();
        }
        else
        {
            errorCount = 0;//since error active, reset error counter
        }
    }
    public void DishNameInput()
    {
        int errorCount = 0;
        if (dishes.text.Length > 0)
        {
         
            dishInput = dishes.text;
            if(dishInput.Contains(","))
            {
                errorCount++;
                errorDish.SetActive(true);
            }
            Debug.Log("the dish: " + dishInput);
        }
        else
        {
            errorCount++;
            errorDish.SetActive(true);
        }
        if(errorCount==0)
        {
            SetBoolDish();
        }
        else
        {
            errorCount = 0;
        }
    }
    private string[] SplitInput(string theInputString)
    {
        string[] toReturn;
        toReturn = theInputString.Split(',');
       

        return toReturn;
    }
    public void SetBoolIngredient()
    {
        if (ingredientsOn)
        {
            ingredientsOn = false;
        }
        else if (!ingredientsOn)
        {
            ingredientsOn = true;
        }
    }
    public void SetBoolDish()
    {
        if (dishOn)
        {
            dishOn = false;
        }
        else if (!dishOn)
        {
            dishOn = true;
        }
    }
    public void ReadyForRequest()
    {
        Debug.Log(dishOn + " " + ingredientsOn);
        if (ingredientsOn && dishOn)
        {
            
            handleRequest.ingredientsNames = ingredientsNames;
            handleRequest.dishName = dishInput;
            handleRequest.index = 3;

        }
        else if (ingredientsOn && !dishOn)
        {
            foreach (string s in ingredientsNames)
            {
                Debug.Log(s);
            }
            handleRequest.ingredientsNames = ingredientsNames;
            handleRequest.index = 1;
            
        }
        else if(dishOn && !ingredientsOn)
        {
            handleRequest.dishName = dishInput;
            handleRequest.index = 2;
        }
        else if(!dishOn && !ingredientsOn)
        {
            errorInput.SetActive(true);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        }
}
 
}
