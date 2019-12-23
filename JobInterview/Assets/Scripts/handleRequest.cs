using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


/*
 * 
 * Allows to send HTML request based on user input 
 * 
 * 
 */
public class HandleRequest : MonoBehaviour
{
    private const string THEURL = "http://www.recipepuppy.com/api/";//this part of the URL is constant
    private string theURL;//the variable part of the URL with either ingredients and dish name or one of the other
    private int pageCounter, pageNumber;//allows to keep track of the pages fetched
    private static int currentPage;
    private List<string[]> pagesObtained;//the list of each page content obtained stored as lists of string. Each string is a recipe and each string[] is a page
    private bool canRequest, done;

    public static int index;
    public GameObject resultPrefab, pageResult,content;//the different gameObject used in the results page
    public TMP_Text pageAmount,pendingResult;
    public TMP_Text[] theCurrentPageText;
    public TMP_InputField pageToSearch;
    public Texture replacement;
    public static string[] ingredientsNames;
    public static string dishName;
    public GameObject errorNoMatch, scrollViewContent, loadingObject, errorPageNoFound, errorPageInput;//the different panels used in the search process

    //Using OnEnable to be able to reset the search parameters each time
    void OnEnable()
    {
        CharacterManager.menuOn = true;
        canRequest = true;
        done = false;
        ingredientsNames = new string[] { };

        pagesObtained = new List<string[]>();
        index = pageCounter = pageNumber = currentPage= 0;

    }
  
    //clears out the input field and resume the paused game
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
            switch (index)//depending on what the user input, a different getRequestInput method call is done
            {
                case 1://search with ingredients only
                    theURL = GetRequestInput(ingredientsNames);
                    theURL += "&p=";
                    index = 0;//getting input info once
                    break;
                case 2://search with dish name only
                    Debug.Log("dish");
                    theURL = GetRequestInput(dishName);
                    theURL += "&p=";
                    index = 0;
                    break;
                case 3://search with both
                    Debug.Log("both");
                    theURL=  GetRequestInput(ingredientsNames, dishName);
                    theURL += "&p=";
                    index = 0;
                    break;
            }
            StartCoroutine(GetRequest(theURL));

        }
        else if (done)//if no more pages to fetch
        {
            done = false;
            DisplayPageResult();     
        }
    }

    //allows to set up URL with ingredients only
    public string GetRequestInput(string[] ingredients)
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

    //allows to set up URL with both ingredients and dish name
    public string GetRequestInput(string[] ingredients, string dish)
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
    //allows to set up URL with dish name only
    public string GetRequestInput(string dish)
    {
        string url;
        dishName = dish;
        url = THEURL + "?q=" + dishName;
       
        return url;

    }
    //sends HTML request with a proper URL based on user input
    IEnumerator GetRequest(string uri)
    {
        Debug.Log("pending...");
        //we request as long as the page returned as some content
        while (canRequest)
        {

            pageCounter += 1;
            string url = uri + (pageCounter);

            pendingResult.text = pagesObtained.Count + " results";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    ErrorNoMatchActive();
                }
                else if(!webRequest.isHttpError)
                {
                
                    string[] pageContent = webRequest.downloadHandler.text.Split('{');
                    //the first two element of the page content are not recipes but just titles so they're ignored
                    if (pageContent.Length > 2)
                    {
                        pagesObtained.Add(pageContent);//one page per index of pagesObtained
                    }
                    else
                    {
                        //if page content empty we end the search
                        loadingObject.SetActive(false);
                        done = true;
                        canRequest = false;
                    }


                }
             
            }
        }
   
    }

    //once search successful and pages stored in pagesobtained list, we can fetch a specific page with this method
    void GetPage(int pageNumber)
    {

        if (pageNumber < pageCounter && pageNumber >= 0)
        {
            DisplayResult(pagesObtained[(pageNumber)]);
        }
  
     }

    //displays how many pages were obtained by the search (excluding pages with errors)
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
    //if no match with our request
    void ErrorNoMatchActive()
    {
        errorNoMatch.SetActive(true);

    }
    //ingredients and recipe title needs reformating before being displayed
    string ReformatTheString(string input)
    {
        string[] theNewString = new string[2];//contains the recipe title and ingredients list (separated by commas)

        //splits the string obtained by HTML request to fetch the recipe title and ingredients list
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

    //if image URL exists we fetch it
    string GetImageURL(string theImageUrl)
    {
        string theImgURL="";
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
        }
        return theImgURL;
    }

    //displays the results properly
    void DisplayResult(string[] theRecipe)
    {
        Debug.Log("CURRENT PAGE = " + currentPage);
        for (int i =2;i<theRecipe.Length;i++) //(string s in theRecipe)
        {
            
            string newString = ReformatTheString(theRecipe[i]);
            string imgURL = GetImageURL(theRecipe[i]);
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
                    //initialise the recipe name and clean it
                    t.gameObject.GetComponent<TMP_Text>().text = elements[0];
                    Debug.Log(t.gameObject.GetComponent<TMP_Text>().text);
                    t.gameObject.GetComponent<TMP_Text>().text = CleanString(t.gameObject.GetComponent<TMP_Text>().text);
                    Debug.Log(t.gameObject.GetComponent<TMP_Text>().text);
                }
                 else if (t.name.Equals("Ingredients"))
                 {
                    t.gameObject.GetComponent<TMP_Text>().text = elements[1];  
                 }
                 else if(t.name.Equals("RawImage"))
                {
                    //if image URL fetched successfully we fetch the image otherwise attribute image replacement
                    if(!imgURL.Equals("null"))
                    {
                        RawImage theImage = t.gameObject.GetComponent<RawImage>();
                        StartCoroutine(GetTheImageTexture(imgURL, theImage));
                    }
                    else
                    {
                        t.gameObject.GetComponent<RawImage>().texture = replacement;
                    }
                }
             }

             //display results on the results page
             theCurrentPageText[0].text = "Results for page " + ((currentPage+1));
             theCurrentPageText[1].text = "Page " + ((currentPage+1));
            
        }

    }
    //cleans the string from the new lines characters and the onpening/closing 
    private string CleanString(string toClean)
    {
        string pattern1 = "\\n";
        string pattern2 = "\\r";
        string pattern3 = "\\t";
        string pattern4 = "\"";
        if (toClean.Contains(pattern1))
        {
            toClean = toClean.Replace(pattern1, "");
        }
        if (toClean.Contains(pattern2))
        {
            toClean = toClean.Replace(pattern2, "");
        }
        if (toClean.Contains(pattern3))
        {
            toClean = toClean.Replace(pattern3, "");
        }
        if (toClean.Contains(pattern4))
        {
            toClean = toClean.Replace(pattern4, "");
        }
        
        return toClean;
    }
    //each recipe has a thumbnail linked to an URL, if thumbnail exist we fetch it
    IEnumerator GetTheImageTexture(string url,RawImage theImg)
    {
       
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
    //get pageNumber from page input field
    public void GetThePageFromUser()
    {
        if (pageToSearch.text.Length > 0)
        {
            pageNumber = int.Parse(pageToSearch.text);
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

    //once page input from user checked, fetch a page for the first time
    public void StartPageSearch()
    {
      
        //pageNumber initialised before hand in getThePageFromUser
        GetPage(pageNumber);
     
    }

    //fetches next page, if index too big switch back to beginning of list
    public void NextPage()
    {
        //clears the layout before fetching a new page
        ClearScrollViewContent();
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

    //fetches previous page if proper index, otherwise switch back to end of list
    public void PreviousPage()
    {
        //clears layout before fetching a new page
        ClearScrollViewContent();
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
    
    //method used after user clicks on previous/next button on result page
    public void StartPageSearch(int pageNum)
    {
        GetPage(pageNum);
    }

    //destroys the current result page layout
    private void ClearScrollViewContent()
    {
        foreach (Transform child in scrollViewContent.transform)
        { 
            GameObject.Destroy(child.gameObject);
        }
    }

}
