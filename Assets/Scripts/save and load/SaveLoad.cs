
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad 
{
    static string path = Application.persistentDataPath + "/Menu.fun";
    
    //call from anywhere
    public static void SaveLevels(){

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);


        UI_Levels_Data data = new UI_Levels_Data();

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static UI_Levels_Data LoadLevels(){
        
        if(File.Exists(path)){

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UI_Levels_Data data = formatter.Deserialize(stream) as UI_Levels_Data;
            stream.Close();

            return data;

        }
        else{
            Debug.LogError("save not found in: " + path);
            return null;
        }

    }
}
