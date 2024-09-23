using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class lets Pets be Serializable, to be saved within the saveload script
/// 
/// Used by the SaveandLoad class
/// </summary>
[System.Serializable]
public class Pets_Data_Serializable
{
    //pets save load stuff:
    public Dictionary<string, bool> pets; //dictionary holding all pets accesability
    public string[] curr_pets; //current pets being used by player

    //constuctor
    public Pets_Data_Serializable()
    {
        pets = PetsAccess.petAccess; //pulls levels access from exisiting level access script
        curr_pets = PetsAccess.current_pets_slotted;
    }
}
