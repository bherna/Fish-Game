using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PetNames {SchoolTeacher, DrCrabs, WhiteKnight, TinyOctopus, Athos, Porthos, Aramis, Khalid, MaryFertile, Salt, Cherry, Missing}

public static class PetsAccess 
{
    //pets access Dictionary
    //each pet is an index in the array
    public static Dictionary<PetNames, bool> petAccess; //entire dictionary list of all pets in game, and their accessability
    public static List<PetNames> current_pets_slotted; //pets the player wants to use next tank


    // ONLY EDIT THIS DICTIONARY IF YOU WANT TO ADD A NEW PET
    // * whenever a new pet is added to our resources folder, we then need to add a reference to it here (that should be it)
    // * the ordering matches how they are displayed in the main menu pet selection screen
    // * ALSO MAKE SURE WE HAVE A "./resources/mainmenu/petswim/{PetName}" obj reference to show (you can duplicate the missing prefab, just change the name) 
    private static Dictionary<PetNames, bool> newGamePetsDictionary = new Dictionary<PetNames, bool>{
            {PetNames.SchoolTeacher, false},
            {PetNames.DrCrabs, false},
            {PetNames.WhiteKnight, false},
            {PetNames.Cherry, false},
            {PetNames.TinyOctopus, false},
            {PetNames.Athos, false},
            {PetNames.Porthos, false},
            {PetNames.Aramis, false},
            {PetNames.Khalid, false},
            {PetNames.MaryFertile, false},
            {PetNames.Salt, false}

        };

    //this should be the default method to be used in any loading
    //pull any saved data loaded within the save path for pet access, if non, create new game pet access
    public static void LoadPets(){

        //make sure we only run this once per game start up
        if(petAccess == null){

            Pets_Data_Serializable data = SaveLoad.Load_Pets();
            
            if(data != null){
                LoadGame(data);
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
        petAccess = newGamePetsDictionary;
        
        //reset slotted pets, else player can have pets preselected pets to take into game
        current_pets_slotted = new List<PetNames>();

    }


    //load game takes the data we are give, assuming the data is not null
    //if the pet dictionary size is different from what was loaded, then we have updated our game for new pets
    //so will need to take that into consideration ation
    private static void LoadGame(Pets_Data_Serializable data){

        //if pet dictionarys are same size, then we can just load normally
        if(data.pets.Count == newGamePetsDictionary.Count){

            petAccess = data.pets;
            current_pets_slotted = data.curr_pets;
        }
        //if our save has a smaller size, then we have to update our saved pets dict to match new pets dict
        else if(data.pets.Count < newGamePetsDictionary.Count){

            var newPetsAccess = newGamePetsDictionary;
            
            //for each pet in our currently loaded pet dictionary
            foreach(KeyValuePair<PetNames, bool> oldPetEntry in data.pets){
                //if our old pet entry is unlocked
                if(oldPetEntry.Value){
                    //then update our new pet dictionary to have it unlocked as well
                    newPetsAccess[oldPetEntry.Key] = true;
                }
                //else it stays locked
            }

            //now we have a new dictionary
            petAccess = newPetsAccess;
            //also reset slotted pets, just in case
            current_pets_slotted = new List<PetNames>();
        }
        //else our loaded pets is bigger than our new dictionary
        //which should not be possible
        else{

            //Send debug message, and set our pets unlock to be empty.
            Debug.Log(string.Format("There are more pets loaded than possible.")); 
            NewGame();  
        }
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
