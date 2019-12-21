using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class handleRequest : MonoBehaviour
{
    private const string THEURL = "http://www.recipepuppy.com/api/";
    public static string[] ingredientsNames;
    public static string dishName;
    public GameObject errorNoMatch,scrollViewContent,sliderObject;
    private string theURL;
    public static int index;
    private int pageCounter, pageNumber,currentPage;
    private List<string[]> pagesObtained;
    private List<int> errors;
    private bool canRequest, done;
    public GameObject resultPrefab, pageResult,content;
    public TMP_Text pageAmount;
    public TMP_Text[] theCurrentPageText;
    public TMP_InputField pageToSearch;
    public Texture replacement;
    
    // Start is called before the first frame update
    void OnEnable()
    {

        canRequest = true;
        done = false;
        ingredientsNames = new string[] { };
        errors = new List<int>();
        pagesObtained = new List<string[]>();
        index = pageCounter = pageNumber = currentPage= 0;

    }
  
    private void OnDisable()
    {
        CharacterMovement.menuOn = false;
        Debug.Log("no menuON");
        //clearScrollViewContent();
        GameManager.instance.Resume();
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
            //Debug.Log(pageCounter);
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
                else if(!webRequest.isHttpError)
                {
                
                    string[] pageContent = webRequest.downloadHandler.text.Split('{');
                    if (pageContent.Length > 2)
                    {
                        pageCounter += 1;
                        pagesObtained.Add(pageContent);//one page per index 
                        if(pageCounter>10)
                        {
                            sliderObject.SetActive(false);
                            done = true;
                            canRequest = false;
                        }
                     
                    }
                    else
                    {
                        //Debug.Log(pageCounter);
                        sliderObject.SetActive(false);
                        done = true;
                        canRequest = false;
                    }


                }
                else
                {
                   Debug.Log("someError found page :"+ pageCounter+1);
                    errors.Add((pageCounter+1));
                    pageCounter++;
                }
            }
        }
   
    }
    void GetPage(int pageNumber)
    {
        
           DisplayResult(pagesObtained[pageNumber]);
     }
       
    

    public void DisplayPageResult()
    {
        if (pageCounter > 0)
        {
            pageResult.SetActive(true);
            int actualPageAmount = pageCounter - errors.Count;
            pageCounter = actualPageAmount-1;
            pageAmount.text = "There are " + pageCounter.ToString()+ " pages";
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
            

        return input;
    }
    string getImageURL(string theImageUrl)
    {
        string theImgURL="";
        //Debug.Log(theImageUrl);
        string[] temp = theImageUrl.Split(',');
        foreach(string s in temp)
        {
            if(s.Contains("thumbnail"))
            {
                string[] theURL = s.Split('"');
                
                if(theURL[3].Contains("jpg"))
                {
                    theImgURL = theURL[3];

                }
                else
                {
                    theImgURL = "null";
                }
            }
        }
        if (!theImgURL.Equals("null"))
        {
            theImgURL = theImgURL.Replace("\\", "");
            Debug.Log("URL: " + theImgURL);
        }
        return theImgURL;
    }
    void DisplayResult(string[] theRecipe)
    {
       
        for(int i =2;i<theRecipe.Length;i++) //(string s in theRecipe)
        {
            
            string newString = reformatTheString(theRecipe[i]);
            string imgURL = getImageURL(theRecipe[i]);
            GameObject newResult = Instantiate(resultPrefab);
             //instantiate the prefab
             newResult.transform.SetParent(content.transform, false);
             newResult.transform.localPosition = Vector3.zero;
             //split s 
             string[] elements = newString.Split(';');
             foreach(Transform t in newResult.transform)
             {
                 if(t.name.Equals("Name"))
                 {
                    elements[0].Replace("\n", "");
                    t.gameObject.GetComponent<TMP_Text>().text = elements[0];
                     //Debug.Log("Name: "+elements[0]);
                 }
                 else if (t.name.Equals("Ingredients"))
                 {
                    t.gameObject.GetComponent<TMP_Text>().text = elements[1];
                     //Debug.Log("Ing: " + elements[1]);
                 }
                 else if(t.name.Equals("RawImage"))
                {
                    if(!imgURL.Equals("null"))
                    {
                        Debug.Log("here");
                        RawImage theImage = t.gameObject.GetComponent<RawImage>();
                        StartCoroutine(getTheImageTexture(imgURL, theImage));
                    }
                    else
                    {
                        t.gameObject.GetComponent<RawImage>().texture = replacement;
                    }
                }
             }

             theCurrentPageText[0].text = "Results for page " + (currentPage);
             theCurrentPageText[1].text = "Page " + (currentPage);
             //assign name and ingredients to prefab element
        }

    }
  
    IEnumerator getTheImageTexture(string url,RawImage theImg)
    {
        Debug.Log("THE URL: "+url);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("BAD URL: " + url);
            Debug.Log(request.error);
        }
        else
        {

            theImg.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
         
         
    }
    public void getThePage()
    {
        if (pageToSearch.text.Length > 0)
        {
            pageNumber = int.Parse(pageToSearch.text);//get pageNumber from input field
            currentPage = (pageNumber+1);
        }

    }
    public void StartPageSearch()
    {
        if (pageNumber < pageCounter)
        {
            GetPage(pageNumber);
        }
        else
        {
            //error
        }
    }
    public void nextPage()
    {
        clearScrollViewContent();
        currentPage++;
        if (currentPage < pageCounter)
        {
            GetPage(currentPage);
        }
        else
        {
            currentPage = 0;
            GetPage(currentPage);
        }
    }
    public void previousPage()
    {
        clearScrollViewContent();
        currentPage--;
        if (currentPage > 0)
        {
            GetPage(currentPage);
        }
        else
        {
            currentPage = (pageCounter - 1);
            GetPage(currentPage);
        }
    }
    public void StartPageSearch(int pageNum)
    {
        GetPage(pageNum);
    }
    private void clearScrollViewContent()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            //Debug.Log("destroying " + child.name);
            GameObject.Destroy(child.gameObject);
        }
    }

}
