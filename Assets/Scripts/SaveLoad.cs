using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad
{
    public static void SaveData(MapStats mapStats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        string path = Application.persistentDataPath + "/" + mapStats.mapName + ".immune";

        FileStream stream = new FileStream(path, FileMode.Create);

        //CharacterData charData = new CharacterData(character);

        formatter.Serialize(stream, mapStats);
        stream.Close();
    }

    public static MapStats LoadData(string mapName)
    {
        string path = Application.persistentDataPath + "/" + mapName + ".immune";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            MapStats data = formatter.Deserialize(stream) as MapStats;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Error: Save file not found in " + path);
            return null;
        }
    }
}