
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad 
{
    static string path_levels = Application.persistentDataPath + "/Levels.fun";
    static string path_pets = Application.persistentDataPath + "/Pets.fun";

    
    //------------------------- save all save types ---------------------------------
    public static void SaveGame(){
        Save_Levels();
        Save_Pets();
    }


    public static void Save_Levels(){

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path_levels, FileMode.Create);

        //save data into data path
        Levels_Data_Serializable data = new Levels_Data_Serializable();

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static void Save_Pets(){

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path_pets, FileMode.Create);

        //save data into data path
        Pets_Data_Serializable data = new Pets_Data_Serializable();

        formatter.Serialize(stream, data);
        stream.Close();

    }


    //-------------------- load all types -----------------------------------------
    public static Levels_Data_Serializable Load_Levels(){
        
        if(File.Exists(path_levels)){

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path_levels, FileMode.Open);

            Levels_Data_Serializable data = formatter.Deserialize(stream) as Levels_Data_Serializable;
            stream.Close();

            return data;

        }
        else{
            Debug.LogError("save not found in: " + path_levels);
            return null;
        }

    }



    public static Pets_Data_Serializable Load_Pets(){
        
        if(File.Exists(path_pets)){

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path_pets, FileMode.Open);

            Pets_Data_Serializable data = formatter.Deserialize(stream) as Pets_Data_Serializable;
            stream.Close();

            return data;

        }
        else{
            Debug.LogError("save not found in: " + path_pets);
            return null;
        }

    }



}
