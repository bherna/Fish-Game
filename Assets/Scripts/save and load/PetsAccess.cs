using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PetNames {SchoolTeacher, DrCrabs, WhiteKnight, TinyOctopus, Athos, Porthos, Aramis, Khalid, MaryFertile, Salt, Missing}

public static class PetsAccess 
{
    //pets access Dictionary
    //each pet is an index in the array
    public static Dictionary<PetNames, bool> petAccess; //entire dictionary list of all pets in game, and their accessability
    public static List<PetNames> current_pets_slotted; //pets the player wants to use next tank

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
                NewGame();
            }
            
        }
    }


    //create a new dictionary for pet access
    public static void NewGame(){

        //Debug.Log("New Pets Dictionary + collection");

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


        current_pets_slotted = new List<PetNames>();

    }



    //--------------------- get and set methods ------------------------------//
    public static bool GetPet_Access(PetNames petName){
        return petAccess[petName];
    }

    //unlock a new pet for player,
    //if new pet (petname) is titled  missing, then we just return
    //          - useful to avoid unlocking new pet
    public static void UnlockPet_Access(PetNames petName){
        
        if(petAccess.ContainsKey(petName))
            {petAccess[petName] = true;}
        
    }


    public static void UpdateSetSelectedPets(List<PetNames> selectedPets){
        current_pets_slotted = selectedPets;
    }
}
