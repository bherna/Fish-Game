using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PetNames {SchoolTeacher, DrCrabs, WhiteKnight, TinyOctopus, Athos, Porthos, Aramis, Khalid, MaryFertile, Salt}

public static class PetsAccess 
{
    //pets access Dictionary
    //each pet is an index in the array
    public static Dictionary<PetNames, bool> petAccess; //entire dictionary list of all pets in game, and their accessability
    public static string[] current_pets_slotted; //pets the player wants to use next tank


    //save current pet access for game
    public static void SavePets_Dictionary(){
        SaveLoad.Save_Pets();
    }


    //this should be the default method to be used in any loading
    //pull any saved data loaded within the save path for pet access, if non, create new game pet access
    public static void LoadPets(){

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

        petAccess = new Dictionary<PetNames, bool>{
            {PetNames.SchoolTeacher, false},
            {PetNames.DrCrabs, false},
            {PetNames.WhiteKnight, false},
            {PetNames.TinyOctopus, false},
            {PetNames.Athos, false},
            {PetNames.Porthos, false},
            {PetNames.Aramis, false},
            {PetNames.Khalid, false},
            {PetNames.MaryFertile, false},
            {PetNames.Salt, false}

        };

    }



    //--------------------- get and set methods ------------------------------//
    public static bool GetPet_Access(PetNames petName){
        return petAccess[petName];
    }

    public static void UnlockPet_Access(PetNames petName){
        petAccess[petName] = true;
    }
}
