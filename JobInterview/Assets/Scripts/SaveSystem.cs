using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
   
    public static List<Game> saved = new List<Game>();
    public static int body, face, legs, arms;
    
    public static void SavePlayer()
    {
        Game.current.thePlayer.bodyIndex = PlayerPrefs.GetInt("bodyIndex");
        Game.current.thePlayer.faceIndex = PlayerPrefs.GetInt("faceIndex");
        Game.current.thePlayer.armsIndex = PlayerPrefs.GetInt("armsIndex");
        Game.current.thePlayer.legsIndex = PlayerPrefs.GetInt("legsIndex");
        Debug.Log(Game.current.thePlayer.bodyIndex+" + " + Game.current.thePlayer.faceIndex + " + " + Game.current.thePlayer.legsIndex + " + " + Game.current.thePlayer.armsIndex);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ThePlayerInfo.gd";
        FileStream file = File.Create(path);
        PlayerData data = new PlayerData
        { 
            bodyIndex = Game.current.thePlayer.bodyIndex,
            legsIndex = Game.current.thePlayer.legsIndex,
            armsIndex= Game.current.thePlayer.armsIndex,
            faceIndex = Game.current.thePlayer.faceIndex,
            
    };
        formatter.Serialize(file, data);//converts player data to binary file
        file.Close();

    }

    public static void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/ThePlayerInfo.gd";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(path, FileMode.Open);
            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();
           
            body = data.bodyIndex;
            face = data.faceIndex;
            legs = data.legsIndex;
            arms = data.armsIndex;
            Debug.Log(body + " " + face + " " + legs + " " + arms);
        }
        else
        {
            Debug.LogError("Save file not found in" + path);

        }


    }
}