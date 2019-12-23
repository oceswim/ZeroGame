using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
   
    //allows to store the outfits created to load them as needed
    public static List<PlayerData> saved = new List<PlayerData>();
    public static int body, face, legs, arms,customisationIndex;
    
    public static void SavePlayer()
    {
        Game.current.thePlayer.bodyIndex = PlayerPrefs.GetInt("bodyIndex");
        Game.current.thePlayer.faceIndex = PlayerPrefs.GetInt("faceIndex");
        Game.current.thePlayer.armsIndex = PlayerPrefs.GetInt("armsIndex");
        Game.current.thePlayer.legsIndex = PlayerPrefs.GetInt("legsIndex");
        Game.current.thePlayer.customisationIndex = saved.Count;

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
        //adds newly saved outfit to our file to be loadable later
        saved.Add(data);
        formatter.Serialize(file, saved);//converts player data to binary file
        file.Close();

    }

    //loads the poper outfit based on index given and fetches outfit in the 'saved' List of playerdata
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

    //fetches the file and loads its data
    public static void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/ThePlayerInfo.gd";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            saved = (List<PlayerData>)formatter.Deserialize(stream);
            stream.Close();

            //load the player with the latest saved outfit
            body = saved[(saved.Count - 1)].bodyIndex;
            face = saved[(saved.Count - 1)].faceIndex;
            legs = saved[(saved.Count - 1)].legsIndex;
            arms = saved[(saved.Count - 1)].armsIndex;
            customisationIndex = saved[(saved.Count - 1)].customisationIndex;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);

        }


    }
}