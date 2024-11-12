using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject slidePanelsParentObj;         //gameobject reference for all ui level panels + main menu
    [SerializeField] EventOnHover_PlayButton playButton; //used in turning the tank lights on
    [SerializeField] GameObject grid_pets;      //all pet ui interactables (for selction)
                                                //we need the recttransform from each button/panel child
    private List<GameObject> pet_list;          //all pets in tank
    private List<PetNames> selectedPets; //up to 3 pets to hold (string name)

    private string tankSceneName; //current tank level selected (name)
    private int curr_screen = 0; //main menu == 0, levels, pet panel == -1


    public static MainMenu instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }

    private void Start() {

        
        //set ui tabs positions 
        for (int i = 0; i < slidePanelsParentObj.transform.childCount; i++){
            slidePanelsParentObj.transform.GetChild(i).gameObject.transform.localPosition = new Vector3(Screen.width*i,0,0); //set their pos
            slidePanelsParentObj.transform.GetChild(i).gameObject.SetActive(true);
        }

        //set last panel (pets)
        //set it to false, since we dont want to see it
        //also avoid moving it since our pet coords are saved from this (0,0) position
        transform.GetChild(transform.childCount-1).gameObject.SetActive(false);

        // pets stuff
        PetsAccess.LoadPets();                          //load saved pets file
        selectedPets = PetsAccess.current_pets_slotted; //update slots
        SpawnPets();                                    //spawn pets
    }


    //Go to scene currSceneSet
    public void PlayLevel(){

        //save selected pets
        PetsAccess.UpdateSetSelectedPets(selectedPets);
        SaveLoad.Save_Pets();

        //start level last
        SceneManager.LoadScene(tankSceneName);
    }



    public void QuitApp(){
        Application.Quit();
        Debug.Log("application has quit.");
    }

    public void Next_UIScreen(){

        for (int i = 0; i < slidePanelsParentObj.transform.childCount; i++){
            slidePanelsParentObj.transform.GetChild(i).gameObject.transform.localPosition -= new Vector3(Screen.width,0,0);
        }

        curr_screen += 1;
    }

    public void Previous_UIScreen(){

        for (int i = 0; i < slidePanelsParentObj.transform.childCount; i++){
            slidePanelsParentObj.transform.GetChild(i).gameObject.transform.localPosition += new Vector3(Screen.width,0,0);
        }

        curr_screen -= 1;

        //logic to reset the play button to turn of the lights in the fish tank
        if(curr_screen == 0){
            playButton.OnPointerReturnToTitleScreen();
        }
    }


    //set scene currSceneSet
    //this lets us move from level select to pet select
    public void GoToPetsPanel(string tankScene, string level){
        
        //save current tank scene name, so we go to that level after selecting pets
        tankSceneName = tankScene;
        //update the level name aswell, used in the controller enemy script for choosing what json enemywaves file
        GameVariables.UpdateLevel(level); 

        //---move to last panell---//
        //disbale current level panel
        //eneable pet panel
        slidePanelsParentObj.transform.GetChild(curr_screen).gameObject.SetActive(false);
        transform.GetChild(transform.childCount-1).gameObject.SetActive(true);
        

        //---have pets move to corresponding ui button location---//
        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pets_InMainMenu>().ToAbility();
        }

        //---update selected pets in ui screen---//

    }


    //if player want to leave pet selection -> return to level selection, return to world panel they were last on
    public void ReturnToLevelSelection(){

        //return to last level panel player was on before
        //enable level panel
        //disable pet panel
        slidePanelsParentObj.transform.GetChild(curr_screen).gameObject.SetActive(true);
        transform.GetChild(transform.childCount-1).gameObject.SetActive(false);

        //let pets return to idle
        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pets_InMainMenu>().ToIdle();
        }

        //update selected pets
        PetsAccess.UpdateSetSelectedPets(selectedPets);

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

        GameObject pet;
        try{
            //spawn pet
            pet = Instantiate(Resources.Load(fileLoc + petName.ToString()) as GameObject, Vector2.zero, Quaternion.identity);

            //Pet_Toggle Button set up
            //update pet name and selection
            //update access on button
            grid_pets.transform.GetChild(i).GetComponent<Pet_ToggleButton>().Init(petName,IfSelected(petName));

            //update pet button to be interactable
            //if we have acces to this pet, set to interactable
            grid_pets.transform.GetChild(i).GetComponent<Button>().interactable = accessable;
            
        }
        catch(Exception){
            
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
