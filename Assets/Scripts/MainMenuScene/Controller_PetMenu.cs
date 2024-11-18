using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_PetMenu : MonoBehaviour
{


    [SerializeField] GameObject grid_pets;      //all pet ui interactables (for selction)
                                                //we need the recttransform from each button/panel child


    private List<GameObject> pet_list;          //all pets in tank
    public List<PetNames> selectedPets {get; private set;} //up to 3 pets to hold (string name)



    //singleton this
    public static Controller_PetMenu instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }




    // Start is called before the first frame update
    private void Start()
    {
        // pets stuff
        PetsAccess.LoadPets();                          //load saved pets file
        selectedPets = PetsAccess.current_pets_slotted; //update slots
        SpawnPets();                                    //spawn pets
    }




    private void SpawnPets(){

        //create a reference
        pet_list = new List<GameObject>();


        //file location of all pets for main menu screen
        string fileLoc = "MainMenu/PetsSwim/";

        //for each pet saved in main menu pets sub folder  -> spawn into tank
        //instance
        int i = 0;
        //the pets spawn order is determined by the pets access dictionary holding them (this could cause a bug in the future)
        foreach(var tup in PetsAccess.petAccess){
            
            //spawn pet, but in function, to avoid missing ones
            SpawnAPet(fileLoc, i, tup.Key, tup.Value);
            i++;
        }

        if(i < grid_pets.transform.childCount){
            grid_pets.transform.GetChild(i).gameObject.SetActive(false);
        }
               
    }


    private void SpawnAPet(string fileLoc, int i, PetNames petName, bool accessable){


        //first get our pet prefab (and make sure its not null)
        GameObject pet = Resources.Load(fileLoc + petName.ToString()) as GameObject;
        
        //if this pet exists, we spawn them,
        //else we use the missing pet obj instead
        if(pet != null){
            pet = Instantiate(Resources.Load(fileLoc + petName.ToString()) as GameObject, Vector2.zero, Quaternion.identity);
            //Pet_Toggle Button set up
            //update pet name and selection
            //update access on button
            grid_pets.transform.GetChild(i).GetComponent<Pet_ToggleButton>().Init(petName,IfSelected(petName));

            //update pet button to be interactable
            //if we have acces to this pet, set to interactable
            grid_pets.transform.GetChild(i).GetComponent<Button>().interactable = accessable;
        }
        else{
            //if pet does not exist,
            //spawn a hidden pet instead
            pet = Instantiate(Resources.Load(fileLoc + "Missing") as GameObject, Vector2.zero, Quaternion.identity);

            //update access on button
            //should just bee false
            //set our pet name to missing aswell
            grid_pets.transform.GetChild(i).GetComponent<Pet_ToggleButton>().Init(PetNames.Missing, false);
            
            //not interactive
            grid_pets.transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
        

        //save pet coords
        //fyi Keep the grid-pets (grind_pets) gameobject grid layout group component disabled, else we can't accesss the actual transform.pos
        var screenPos = grid_pets.transform.GetChild(i).transform.position;                             //get button on ui pos
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, - Camera.main.transform.position);     //convert z to FOV position 
        Vector3 pos = Camera.main.ScreenToWorldPoint(screenPos);                                        //convert to world pos
        pet.GetComponent<Pets_InMainMenu>().SetCoords(pos);

        //add to list and increment
        pet_list.Add(pet);
        
    }





    // ---------------------  reference functions -------------------------------
    //resets the pets in the tank, use when we new game
    public void ResetPets(){

        foreach(GameObject pet in pet_list){
            Destroy(pet);
        }
        selectedPets = PetsAccess.current_pets_slotted; //update slots again
        SpawnPets();                                    //spawn pets again
    }

    //each pet will move to their corelated button location 
    public void PetsToButton(){

        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pets_InMainMenu>().ToAbility();
        }
    }
    public void PetsToIdle(){

        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pets_InMainMenu>().ToIdle();
        }
    }


    //function only allows upto 3 selected pets
    public bool Select_Add(PetNames pet){

        //make sure we are not over adding pets
        //max pets should be 3 (for now) 
        if(selectedPets.Count < 3){
            //add
            selectedPets.Add(pet);
            return true;
        }
        else{
            Debug.Log("More than 3 pets in collection");
            return false;
        }

        

    }

    //removes pet from collection
    public bool Select_Sub(PetNames pet){

        //make sure pet is in list
        if(!selectedPets.Remove(pet)){
            Debug.Log("Pet was not part of collection");
            return false;
        }
        return true;
    }

    //return true if the pet name passed is in our petnames array
    private bool IfSelected(PetNames pet){

        foreach(PetNames petNames in selectedPets){
            if(petNames == pet){
                return true;
            }
        }
        
        //else
        return false;
    }
}
