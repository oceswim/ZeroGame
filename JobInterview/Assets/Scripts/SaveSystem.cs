using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
   
    //
    public static List<PlayerData> saved = new List<PlayerData>();
    public static int body, face, legs, arms,customisationIndex;
    
    public static void SavePlayer()
    {
        Game.current.thePlayer.bodyIndex = PlayerPrefs.GetInt("bodyIndex");
        Game.current.thePlayer.faceIndex = PlayerPrefs.GetInt("faceIndex");
        Game.current.thePlayer.armsIndex = PlayerPrefs.GetInt("armsIndex");
        Game.current.thePlayer.legsIndex = PlayerPrefs.GetInt("legsIndex");
        Game.current.thePlayer.customisationIndex = saved.Count;

        Debug.Log("Current numbers : "+Game.current.thePlayer.bodyIndex+" + " + Game.current.thePlayer.faceIndex + " + " + Game.current.thePlayer.legsIndex + " + " + Game.current.thePlayer.armsIndex +"+ index"+ Game.current.thePlayer.customisationIndex);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ThePlayerInfo.gd";
        FileStream file = File.Create(path);
        PlayerData data = new PlayerData
        {
            bodyIndex = Game.current.thePlayer.bodyIndex,
            legsIndex = Game.current.thePlayer.legsIndex,
            armsIndex = Game.current.thePlayer.armsIndex,
            faceIndex = Game.current.thePlayer.faceIndex,
            customisationIndex = Game.current.thePlayer.customisationIndex
            
        };
        saved.Add(data); 
        foreach (PlayerData s in saved)
        {
            Debug.Log("Saved At index :" + s.customisationIndex + " bodyindex :" + s.bodyIndex + "faceindex: " + s.faceIndex + "armsIndex " + s.armsIndex + "legsIndex " + s.legsIndex);
        }
        formatter.Serialize(file, saved);//converts player data to binary file
        file.Close();

    }

    public static int[] LoadOutfit(int index)
    {
        int[] theIndexes=new int[4];
        Game.current.thePlayer.customisationIndex = PlayerPrefs.GetInt("customisationIndex");
        theIndexes[0] = saved[index].bodyIndex;
        theIndexes[1] = saved[index].faceIndex;
        theIndexes[2] = saved[index].armsIndex;
        theIndexes[3] = saved[index].legsIndex;

        return theIndexes;

    }
    public static void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/ThePlayerInfo.gd";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            saved = (List<PlayerData>)formatter.Deserialize(stream);
            stream.Close();
            
            body = saved[(saved.Count - 1)].bodyIndex;//load the player with the latest saved outfit
            face = saved[(saved.Count - 1)].faceIndex;
            legs = saved[(saved.Count - 1)].legsIndex;
            arms = saved[(saved.Count - 1)].armsIndex;
            customisationIndex = saved[(saved.Count - 1)].customisationIndex;
            foreach (PlayerData s in saved)
            {
                Debug.Log("Loaded: At index :" + s.customisationIndex + " bodyindex :" + s.bodyIndex + "faceindex: " + s.faceIndex + "armsIndex " + s.armsIndex + "legsIndex " + s.legsIndex+" saved.con"+saved.Count);
            }
        }
        else
        {
            Debug.LogError("Save file not found in" + path);

        }


    }
}