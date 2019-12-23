using TMPro;
using UnityEngine;

/*
 * Handles the input from the user who wants to search 
 * a recipe either by ingredients list or with a dish name or both.
 */ 
public class HandleInputs : MonoBehaviour
{
    public TMP_InputField ingredients, dishes;
    private bool dishOn, ingredientsOn;
    private string[] ingredientsNames;
    private string dishInput;
    public GameObject errorInput, errorIngredient, errorDish;//errors related to inputs

    /*
     * Using onenable and ondisable in order to reset properly the inputs fields
     */
    void OnEnable()
    {
        ingredientsNames = new string[] { };
        dishInput = "";
        dishOn = ingredientsOn = false;//recognizes if the search done with ingredients or dishes only or both
    }
    private void OnDisable()
    {
        ingredients.text = dishes.text = "";
    }

    //called if ingredients entered and checks if commas entered
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

    //called if dish name entered and checks if a single name is entered
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
        }
        else
        {
            errorCount++;
            errorDish.SetActive(true);
        }
        if(errorCount==0)//if no error
        {
            SetBoolDish();
        }
       
    }

    //splits the ingredients into a list using the commas
    private string[] SplitInput(string theInputString)
    {
        string[] toReturn;
        toReturn = theInputString.Split(',');
       

        return toReturn;
    }


    //allows to keep track if ingredients are used or not
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
    //allows to keep track if dishes are used or not
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

    //once proper input recognised sends info to handlerequest based on the input received
    public void ReadyForRequest()
    {
        Debug.Log(dishOn + " " + ingredientsOn);
        if (ingredientsOn && dishOn)
        {
            
            HandleRequest.ingredientsNames = ingredientsNames;
            HandleRequest.dishName = dishInput;
            HandleRequest.index = 3;

        }
        else if (ingredientsOn && !dishOn)
        {
            foreach (string s in ingredientsNames)
            {
                Debug.Log(s);
            }
            HandleRequest.ingredientsNames = ingredientsNames;
            HandleRequest.index = 1;
            
        }
        else if(dishOn && !ingredientsOn)
        {
            HandleRequest.dishName = dishInput;
            HandleRequest.index = 2;
        }
        else if(!dishOn && !ingredientsOn)
        {
            errorInput.SetActive(true);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
        }
}
 
}
