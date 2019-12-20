using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class handleRequest : MonoBehaviour
{
    private bool requestDish, requestIngredient;
    public static string[] ingredientsNames;
    public static string dishName;
    public GameObject errorNoMatch;
    private string theURL;
    public static int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        requestDish = requestIngredient = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(index>0)
        {
            switch(index)
            {
                case 1://search with ingredients only
                    getRequestInput(ingredientsNames);
                    break;
                case 2://search with dish name only
                    getRequestInput(dishName);
                    break;
                case 3://search with both
                    getRequestInput(ingredientsNames, dishName);
                    break;
            }
        }
    }
    public void getRequestInput(string[] ingredients)
    {
        string theIngredients = "";
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (i == (ingredients.Length - 1))
            {

                theIngredients += ingredients[i];
            }
            else
            {
                theIngredients += ingredients[i] + ",";
            }
        }

        theURL= "http://www.recipepuppy.com/api/?i="+theIngredients;
    }
    public void getRequestInput(string[] ingredients, string dish)
    {
        ingredientsNames = ingredients;
        dishName = dish;
    }
    public void getRequestInput(string dish)
    {
        dishName = dish;
        theURL = "http://www.recipepuppy.com/api/?q="+dishName;

    }
    IEnumerator GetRequest(string uri)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;

        if (webRequest.isNetworkError)
        {
            errorNoMatchActive();
        }
        else
        {

            //INPUT INFOS HERE
            string[] words = webRequest.downloadHandler.text.Split('{');
            for (int i =0;i<words.Length;i++)
            {
                Debug.Log(words[i]);
            }
        }


    }
    void errorNoMatchActive()
    {

        float timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
        }
        errorNoMatch.SetActive(true);
        

    }
}
