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
    public GameObject errorNoMatch,scrollViewContent,sliderObject,errorPageNoFound,errorPageInput;
    private string theURL;
    public static int index;
    private int pageCounter, pageNumber;
    private static int currentPage;
    private List<string[]> pagesObtained;
    private bool canRequest, done;
    public GameObject resultPrefab, pageResult,content;
    public TMP_Text pageAmount;
    public TMP_Text[] theCurrentPageText;
    public TMP_InputField pageToSearch;
    public Texture replacement;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        CharacterManager.menuOn = true;
        canRequest = true;
        done = false;
        ingredientsNames = new string[] { };

        pagesObtained = new List<string[]>();
        index = pageCounter = pageNumber = currentPage= 0;

    }
  
    private void OnDisable()
    {
        pageToSearch.text = "";
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

            pageCounter += 1;
            string url = uri + (pageCounter);

            Debug.Log("pending..." +url);
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
                        pagesObtained.Add(pageContent);//one page per index 
                    }
                    else
                    {
                        //if request goes through but page content empty
                        sliderObject.SetActive(false);
                        done = true;
                        canRequest = false;
                    }


                }
             
            }
        }
   
    }
    void GetPage(int pageNumber)
    {

        if (pageNumber < pageCounter && pageNumber >= 0)
        {
            DisplayResult(pagesObtained[(pageNumber)]);
        }
  
     }
    public void DisplayPageResult()
    {
        if (pagesObtained.Count > 0)
        {
            pageResult.SetActive(true);
            int actualPageAmount = pagesObtained.Count;
            pageCounter = actualPageAmount;
            pageAmount.text = "There are " + pageCounter.ToString()+ " pages";
        }
        else
        {
            errorPageNoFound.SetActive(true);
            gameObject.SetActive(false);//deactivates requester to reset research

        }
    }
    void errorNoMatchActive()
    {
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
        Debug.Log("CURRENT PAGE = " + currentPage);
        for (int i =2;i<theRecipe.Length;i++) //(string s in theRecipe)
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
                    elements[0]=elements[0].Replace("\n", "");
                    elements[0]=elements[0].Replace("\t", "");
                    elements[0]=elements[0].Replace("\r", "");
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
                        
                        RawImage theImage = t.gameObject.GetComponent<RawImage>();
                        StartCoroutine(getTheImageTexture(imgURL, theImage));
                    }
                    else
                    {
                        t.gameObject.GetComponent<RawImage>().texture = replacement;
                    }
                }
             }

             theCurrentPageText[0].text = "Results for page " + ((currentPage+1));
             theCurrentPageText[1].text = "Page " + ((currentPage+1));
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
    public void getThePageFromUser()
    {
        if (pageToSearch.text.Length > 0)
        {
            pageNumber = int.Parse(pageToSearch.text);//get pageNumber from input field
            if (pageNumber > 0 && pageNumber <= pageCounter)
            {
                pageNumber = pageNumber - 1;
                currentPage = pageNumber;
                Debug.Log("CURRENT PAGE = " + currentPage);
            }
            else
            {
                errorPageInput.SetActive(true);
            }
           
        }
        else
        {
            errorPageInput.SetActive(true);
        }

    }
    public void StartPageSearch()
    {
      
        GetPage(pageNumber);
     
    }//after user input the page he wants to consult use this method
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
        if (currentPage>=0)
        {
            GetPage(currentPage);
        }
        else
        {
            currentPage = (pageCounter-1);
            GetPage(currentPage);
        }
    }
    public void StartPageSearch(int pageNum)
    {
        GetPage(pageNum);//once user entered proper page number we can start fetching the page he asks for
    }//when user clicks on previous or next button, use this method
    private void clearScrollViewContent()
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            //Debug.Log("destroying " + child.name);
            GameObject.Destroy(child.gameObject);
        }
    }

}
