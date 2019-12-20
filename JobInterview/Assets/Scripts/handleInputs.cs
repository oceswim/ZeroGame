﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handleInputs : MonoBehaviour
{
    public InputField ingredients, dishes;
    private bool dishOn, ingredientsOn;
    private string[] ingredientsInput;
    private string dishInput;
   
    // Start is called before the first frame update
    void Start()
    {
        dishOn = ingredientsOn = false;
    }

    // Update is called once per frame
    public void IngredientsInput()
    {
        if (ingredients.text.Length > 0)
        {
            if (ingredients.text.Contains(","))
            {
                ingredientsInput = splitInput(ingredients.text);
            }
            else if (ingredients.text.Contains(" "))
            {
                //error input type
            }
            else
            { 
                //remove previously added ingredients
                for(int i =0; i<ingredientsInput.Length;i++)
               {
                  ingredientsInput[i].Remove(0);
                            
                }
                
                ingredientsInput[0] = (ingredients.text);
            }
        }
        else
        {
            //error
        }
    }
    public void DishNameInput()
    {
        if (dishes.text.Length > 0)
        {
            dishInput = dishes.text;
            Debug.Log("the dish: " + dishInput);
        }
        else
        {
            //error
        }
    }
    private string[] splitInput(string theInputString)
    {
        string[] toReturn;
        toReturn = theInputString.Split(',');
        foreach (string s in toReturn)
        {
            Debug.Log(s);
        }

        return toReturn;
    }
    public void setBoolIngredient()
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
    public void readyForRequest()
    {
        if (ingredientsOn && dishOn)
        {
            handleRequest.ingredientsNames = ingredientsInput;
            handleRequest.dishName = dishInput;
            handleRequest.index = 3;

        }
        else if (ingredientsOn && !dishOn)
        {
            handleRequest.ingredientsNames = ingredientsInput;
            handleRequest.index = 1;
            
        }
        else if(dishOn && !ingredientsOn)
        {
            handleRequest.dishName = dishInput;
            handleRequest.index = 2;
        }
}
    public void setBoolDish()
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
}
