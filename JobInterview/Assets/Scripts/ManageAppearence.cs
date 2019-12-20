using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManageAppearence : MonoBehaviour
{
    public enum AppearenceDetail
    {
        BODY_MODEL,
        FACE_MODEL,
        ARMS_MODEL,
        LEGS_MODEL
    }

    [SerializeField]
    private Material[] Models;
    [SerializeField]
    private GameObject bodyAnchor;
    [SerializeField]
    private GameObject faceAnchor;
    [SerializeField]
    private GameObject[] armsAnchor;
    [SerializeField]
    private GameObject[] legsAnchor;

    Material activeBody, activeFace, activeArms, activeLegs;
    int materialSize, bodyIndex, faceIndex, legsIndex,armsIndex;
    public TMP_Text[] titles;
    private void Start()
    {
        if (titles.Length > 0)
        {
            titles[0].text = "Body";
            titles[1].text = "Face";
            titles[2].text = "Legs";
            titles[3].text = "Arms";
        }

        if (PlayerPrefs.HasKey("saved"))
        {
            ApplyModification(AppearenceDetail.BODY_MODEL, PlayerPrefs.GetInt("bodyIndex"));
            ApplyModification(AppearenceDetail.FACE_MODEL, PlayerPrefs.GetInt("faceIndex"));
            ApplyModification(AppearenceDetail.LEGS_MODEL, PlayerPrefs.GetInt("legsIndex"));
            ApplyModification(AppearenceDetail.ARMS_MODEL, PlayerPrefs.GetInt("armsIndex"));
        }
        else
        {

            ApplyModification(AppearenceDetail.BODY_MODEL, 0);
            ApplyModification(AppearenceDetail.FACE_MODEL, 0);
            ApplyModification(AppearenceDetail.LEGS_MODEL, 0);
            ApplyModification(AppearenceDetail.ARMS_MODEL, 0);
        }
        materialSize = Models.Length;

    }
    
    public void PlusIndex(int index)//when click right arrow increases list index
    {
        switch (index)
        {
            case 1://body menu
                if (bodyIndex == (materialSize-1))
                {
                    bodyIndex = 0;
                }
                else
                {
                    bodyIndex++;
                }
                titles[0].text = "Body " + (bodyIndex + 1);
                ApplyModification(AppearenceDetail.BODY_MODEL, bodyIndex);
                break;
            case 2://face menu
                if (faceIndex == (materialSize - 1))
                {
                    faceIndex = 0;
                }
                else
                {
                    faceIndex++;
                }
                titles[1].text = "Face " + (faceIndex + 1);
                ApplyModification(AppearenceDetail.FACE_MODEL, faceIndex);
                break;
            case 3://legs menu
                if (legsIndex == (materialSize - 1))
                {
                    legsIndex = 0;
                }
                else
                {
                    legsIndex++;
                }
                titles[2].text = "Legs " + (legsIndex + 1);
                ApplyModification(AppearenceDetail.LEGS_MODEL, legsIndex);
                break;
            case 4://arms menu
                if (armsIndex == (materialSize - 1))
                {
                    armsIndex = 0;
                }
                else
                {
                    armsIndex++;
                }
                titles[3].text = "Arms " + (armsIndex + 1);
                ApplyModification(AppearenceDetail.ARMS_MODEL, armsIndex);
                break;

        }
    }
    public void MinusIndex(int index)//when click left arrow decrease list index
    {
        switch (index)
        {
            case 1://body menu
                if (bodyIndex == 0)
                {
                    bodyIndex = (materialSize - 1);
                }
                else
                {
                    bodyIndex--;
                }
                titles[0].text = "Body " + (bodyIndex + 1);
                ApplyModification(AppearenceDetail.BODY_MODEL, bodyIndex);
                break;
            case 2://face menu
                if (faceIndex == 0)
                {
                    faceIndex = (materialSize - 1);
                }
                else
                {
                    faceIndex--;
                }
                titles[1].text = "Face " + (faceIndex + 1);
                ApplyModification(AppearenceDetail.FACE_MODEL, faceIndex);
                break;
            case 3://legs menu
                if (legsIndex == 0)
                {
                    legsIndex = (materialSize - 1);
                }
                else
                {
                    legsIndex--;
                }
                titles[2].text = "Legs " + (legsIndex + 1);
                ApplyModification(AppearenceDetail.LEGS_MODEL, legsIndex);
                break;
            case 4://arms menu
                if (armsIndex == 0)
                {
                    armsIndex = (materialSize - 1);
                }
                else
                {
                    armsIndex--;
                }
                titles[3].text = "Arms " + (armsIndex + 1);
                ApplyModification(AppearenceDetail.ARMS_MODEL, armsIndex);
                break;

        }
    }
    void ApplyModification(AppearenceDetail detail, int id)
    {
        switch (detail)
        {

            case AppearenceDetail.BODY_MODEL:

                activeBody = Models[id];
                bodyIndex = id;
                bodyAnchor.GetComponent<SkinnedMeshRenderer>().material = activeBody;
                
                
                break;
            case AppearenceDetail.FACE_MODEL:
                activeFace = Models[id];
                faceIndex = id;
                faceAnchor.GetComponent<SkinnedMeshRenderer>().material = activeFace;
                

                break;
            case AppearenceDetail.LEGS_MODEL:
                activeLegs = Models[id];
                legsIndex = id;
                foreach (GameObject g in legsAnchor)
                {
                    g.GetComponent<SkinnedMeshRenderer>().material= activeLegs;
                }
                break;
            case AppearenceDetail.ARMS_MODEL:
                activeArms = Models[id];
                armsIndex = id;
                foreach (GameObject j in armsAnchor)
                {
                    j.GetComponent<SkinnedMeshRenderer>().material= activeArms;
                }

                break;

        }
    }
    public void Save()
    {

        PlayerPrefs.SetInt("bodyIndex", bodyIndex);
        PlayerPrefs.SetInt("faceIndex", faceIndex);
        PlayerPrefs.SetInt("legsIndex", legsIndex);
        PlayerPrefs.SetInt("armsIndex", armsIndex);
        PlayerPrefs.SetString("saved", "true");
        //saveSystem.SavePlayer();

    }
    public void Revert()
    {
        PlayerPrefs.SetInt("bodyIndex", 0);
        PlayerPrefs.SetInt("faceIndex", 0);
        PlayerPrefs.SetInt("legsIndex", 0);
        PlayerPrefs.SetInt("armsIndex", 0);
        ApplyModification(AppearenceDetail.BODY_MODEL, 0);
        ApplyModification(AppearenceDetail.FACE_MODEL, 0);
        ApplyModification(AppearenceDetail.ARMS_MODEL, 0);
        ApplyModification(AppearenceDetail.LEGS_MODEL, 0);


    }
}
