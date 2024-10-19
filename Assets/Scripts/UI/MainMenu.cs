using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] int rectTransform_width = 1920;
    [SerializeField] EventOnHover_PlayButton playButton;
    [SerializeField] GameObject grid_pets; //we need the recttransform from each button/panel child
    private List<GameObject> pet_list;

    private PetNames[] selectedPets; //up to 3 pets to hold (string name)
    private string sceneLevelSet; //level name
    private int curr_screen = 0; //main menu == 0, levels, pet panel == -1



    private void Start() {
        
        //set ui tabs positions (minus the last one)
        for (int i = 0; i < transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition = new Vector3(rectTransform_width*i,0,0); //set their pos
            transform.GetChild(i).gameObject.SetActive(true);
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
        PetsAccess.SetSelectedPets(selectedPets);
        SaveLoad.Save_Pets();

        //start level last
        SceneManager.LoadScene(sceneLevelSet);
    }



    public void QuitApp(){
        Application.Quit();
        Debug.Log("application has quit.");
    }

    public void Next_UIScreen(){

        for (int i = 0; i < transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition -= new Vector3(rectTransform_width,0,0);
        }

        curr_screen += 1;
    }

    public void Previous_UIScreen(){

        for (int i = 0; i < transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition += new Vector3(rectTransform_width,0,0);
        }

        curr_screen -= 1;

        //logic to reset the play button to turn of the lights in the fish tank
        if(curr_screen == 0){
            playButton.OnPointerReturnToTitleScreen();
        }
    }


    //set scene currSceneSet
    //this lets us move from level select to pet select
    public void GoToPetsPanel(string sceneLevelName){
        
        //save current level name, so we go to that level after selecting pets
        sceneLevelSet = sceneLevelName;

        //---move to last panell---//
        //disbale current level panel
        //eneable pet panel
        transform.GetChild(curr_screen).gameObject.SetActive(false);
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
        transform.GetChild(curr_screen).gameObject.SetActive(true);
        transform.GetChild(transform.childCount-1).gameObject.SetActive(false);

        //let pets return to idle
        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pets_InMainMenu>().ToIdle();
        }

        //update selected pets
        PetsAccess.SetSelectedPets(selectedPets);

    }



    private void SpawnPets(){

        //create a reference
        pet_list = new List<GameObject>();


        //file location of all pets for main menu screen
        string fileLoc = "MainMenu/PetsSwim/";

        //for each pet saved in main menu pets sub folder  -> spawn into tank
        //instance
        int i = 0;
        foreach(PetNames petName in PetsAccess.petAccess.Keys){
            
            //spawn pet, but in function, to avoid missing ones
            SpawnAPet(fileLoc, petName, i);
            i++;
        }
               
    }


    private void SpawnAPet(string fileLoc, PetNames petName, int i){

        GameObject pet;
        try{
            //spawn pet
            pet = Instantiate(Resources.Load(fileLoc + petName.ToString()) as GameObject, Vector2.zero, Quaternion.identity);
        }
        catch(Exception){
            
            //if pet does not exist,
            //spawn a hidden pet instead
            pet = Instantiate(Resources.Load(fileLoc + "Hidden") as GameObject, Vector2.zero, Quaternion.identity);
        }
        

        //save pet coords
        Debug.Log(grid_pets.transform.GetChild(i).gameObject.ToString());
        var screenPos = grid_pets.transform.GetChild(i).transform.position;                             //get button on ui pos
        //Debug.Log(petName.ToString()+ "screen pos :" +screenPos.ToString());
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, - Camera.main.transform.position);     //convert z to FOV position 
        Vector3 pos = Camera.main.ScreenToWorldPoint(screenPos);                                        //convert to world pos
        //Debug.Log(petName.ToString()+ "new screen pos :" +screenPos.ToString());
        //Debug.Log(petName.ToString()+" pos: "+pos.ToString());
        pet.GetComponent<Pets_InMainMenu>().SetCoords(pos);

        //add to list and increment
        pet_list.Add(pet);
    }
}
