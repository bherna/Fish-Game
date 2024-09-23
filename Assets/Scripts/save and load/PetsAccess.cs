using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PetsAccess 
{
    //pets access Dictionary
    //each pet is an index in the array
    public static Dictionary<string, bool> petAccess; //entire dictionary list of all pets in game, and their accessability
    public static string[] current_pets_slotted; //pets the player wants to use next tank


    //save current pet access for game
    public static void SavePets_Dictionary(){
        SaveLoad.SaveGame();
    }


    //this should be the default method to be used in any loading
    //pull any saved data loaded within the save path for pet access, if non, create new game pet access
    public static void LoadPets_Dictionary(){

        //make sure we only run this once per game start up
        if(petAccess == null){

            Pets_Data_Serializable data = SaveLoad.Load_Pets();
            
            if(data != null){
                //then load Pets
                petAccess = data.pets;
                current_pets_slotted = data.curr_pets;
            }
            else{
                //then we are a new game pets dictionary
                NewPets_Dict();
            }
            
        }
    }


    //create a new dictionary for pet access
    private static void NewPets_Dict(){

        Debug.Log("New Pets Dictionary.");

        //create a new dictionary with all pets as a new entry , with false access
        petAccess = new Dictionary<string, bool>{
            {"School Teacher", false},
            {"Dr. Crabs", false},
            {"White Knight", false},
            {"Tiny Octopus", false},
            {"Athos", false},
            {"Porthos", false},
            {"Aramis", false},

        };

    }



    //--------------------- get and set methods ------------------------------//
    public static bool GetPet_Access(string petName){
        return petAccess[petName];
    }

    public static void UnlockPet_Access(string petName){
        petAccess[petName] = true;
    }
}
