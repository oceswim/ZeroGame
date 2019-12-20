using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class handleRequest : MonoBehaviour
{
    private const string THEURL = "http://www.recipepuppy.com/api/";
    public static string[] ingredientsNames;
    public static string dishName;
    public GameObject errorNoMatch;
    private string theURL;
    public static int index;
    private int pageCounter, pageNumber;
    private List<string> allRecipes;
    private bool canRequest, done;
    public GameObject resultPrefab, pageResult,content;
    public TMP_Text pageAmount;
    public TMP_InputField pageToSearch;
    // Start is called before the first frame update
    void Start()
    {

        canRequest = true;
        done = false;
        ingredientsNames = new string[] { };
        allRecipes = new List<string>();
        index = pageCounter = pageNumber = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (index > 0)
        {
            switch (index)
            {
                case 1://search with ingredients only
                    theURL = getRequestInput(ingredientsNames);
                    theURL += "&p=";
                    index = 0;//getting input info once
                    break;
                case 2://search with dish name only
                    Debug.Log("dish");
                    theURL = getRequestInput(dishName);
                    theURL += "&p=";
                    index = 0;
                    break;
                case 3://search with both
                    Debug.Log("both");
                    theURL=  getRequestInput(ingredientsNames, dishName);
                    theURL += "&p=";
                    index = 0;
                    break;
            }


            StartCoroutine(GetRequest(theURL));

        }
        else if (done)
        {
            done = false;
            Debug.Log(pageCounter);
            DisplayPageResult();     
        }
    }

    public string getRequestInput(string[] ingredients)
    {
        string theIngredients = "";
   
        string url;
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

        url = THEURL + "?i=" + theIngredients;
        return url;
    }
    public string getRequestInput(string[] ingredients, string dish)
    {
        string theIngredients = "";
        string url;
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

        url = THEURL + "?i=" + theIngredients+"&q="+dish;
        Debug.Log(url);
        return url;
    }
    public string getRequestInput(string dish)
    {
        string url;
        dishName = dish;
        url = THEURL + "?q=" + dishName;
       
        return url;

    }
    IEnumerator GetRequest(string uri)
    {
        Debug.Log("pending...");
        while (canRequest)
        {
            string url = uri + (pageCounter + 1);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    errorNoMatchActive();
                }
                else
                {
                    string[] pageContent = webRequest.downloadHandler.text.Split('{');
                    if (pageContent.Length > 2)
                    {
                        pageCounter += 1;
                        //TEMPORARY
                        if (pageCounter == 20)
                        {
                            done = true;
                            canRequest = false;
                        }
                    }
                    else
                    {
                        //Debug.Log(pageCounter);
                        done = true;
                        canRequest = false;
                    }


                }
            }
        }
        Debug.Log("ok");
    }
    IEnumerator GetPage(int pageNumber)
    {
        

        string url = theURL + pageNumber;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError)
            {
                errorNoMatchActive();
            }
            else
            {
                string[] pageContent = webRequest.downloadHandler.text.Split('{');
                for (int i = 2; i < pageContent.Length; i++)
                {
                    //Debug.Log(pageContent[i]);
                    allRecipes.Add(pageContent[i]);
                }

                DisplayResult(allRecipes);

            }
        }
    }

    public void DisplayPageResult()
    {
        if (pageCounter > 0)
        {
            pageResult.SetActive(true);
            pageAmount.text = "There are " + pageCounter.ToString() + " pages";
        }
        else
        {
            //error;
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
    string reformatTheString(string input)
    {
        string[] theNewString = new string[2];
        string[] temporaryString = input.Split(':');
            for (int i = 0; i < temporaryString.Length; i++)
            {
                if (temporaryString[i].Contains("title"))
                {
                    string[] temporary = temporaryString[i + 1].Split(',');

                    theNewString[0] = temporary[0];
                }
                else if (temporaryString[i].Contains("ingredients"))
                {
                    string[] temporary = temporaryString[i + 1].Split('"');
                    theNewString[1] = temporary[1];
                }

            }

            input = theNewString[0] + ";" + theNewString[1];
            Debug.Log("REFORM: "+input);
        //DisplayResult(input[x]);
        return input;

    }
    void DisplayResult(List<string> theRecipe)
    {
        foreach(string s in theRecipe)
        {
            string newString = reformatTheString(s);
            GameObject newResult = Instantiate(resultPrefab);
            //instantiate the prefab
            newResult.transform.SetParent(content.transform, false);
            newResult.transform.localPosition = Vector3.zero;
            //split s 
            string[] elements = new string[] { };
            elements = newString.Split(';');
            foreach(Transform t in newResult.transform)
            {
                if(t.name.Equals("Name"))
                {
                   t.gameObject.GetComponent<TMP_Text>().text = elements[0];
                    Debug.Log("Name: "+elements[0]);
                }
                else if (t.name.Equals("Ingredients"))
                {
                   t.gameObject.GetComponent<TMP_Text>().text = elements[1];
                    Debug.Log("Ing: " + elements[1]);
                }
            }

            
            //assign name and ingredients to prefab element
        }

    }
  

    public void getThePage()
    {
        if (pageToSearch.text.Length > 0)
        {
            pageNumber = int.Parse(pageToSearch.text);
        }

    }
    public void startPageSearch()
    {
        if (pageNumber < pageCounter)
        {
            StartCoroutine(GetPage(pageNumber));
        }
        else
        {
            //error
        }
    }

  
}
