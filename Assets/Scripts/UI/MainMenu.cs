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

    private string[] selectedPets; //up to 3 pets to hold (string name)


    private int curr_screen = 0;

    private string currSceneSet;



    private void Start() {
        
        //set ui tabs positions
        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.transform.localPosition = new Vector3(rectTransform_width*i,0,0); //set their pos
            transform.GetChild(i).gameObject.SetActive(true);
        }

        // pets stuff
        PetsAccess.LoadPets();                          //load saved pets file
        selectedPets = PetsAccess.current_pets_slotted; //update slots
        SpawnPets();                                    //spawn pets
    }


    //Go to scene currSceneSet
    public void PlayLevel(){

        //save selected pets
        SaveLoad.Save_Pets();

        //start level last
        SceneManager.LoadScene(currSceneSet);
    }



    public void QuitApp(){
        Application.Quit();
        Debug.Log("application has quit.");
    }

    public void Next_UIScreen(){

        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.transform.localPosition -= new Vector3(rectTransform_width,0,0);
        }

        curr_screen += 1;
    }

    public void Previous_UIScreen(){

        for (int i = 0; i < transform.childCount; i++){
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
        currSceneSet = sceneLevelName;

        //---move to last panell---//
        //number of transistions until last scene
        int lastScene = transform.childCount - (curr_screen+1);

        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.transform.localPosition -= new Vector3(rectTransform_width * lastScene,0,0);
        }

        //---have pets move to corresponding ui button location---//
        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pets_InMainMenu>().ToAbility();
        }

        //---update selected pets in ui screen---//

    }


    //if player want to leave pet selection -> return to level selection, return to world panel they were last on
    public void ReturnToLevelSelection(){

        //return to last level panel player was on before
        for (int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.transform.localPosition = new Vector3(rectTransform_width * curr_screen,0,0);
        }

        //save pets selections for next time
        SaveLoad.Save_Pets();

        //let pets return to idle
        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pets_InMainMenu>().ToIdle();
        }

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
            return;
        }
        

        //save pet coords
        var screenPos = grid_pets.transform.GetChild(i).transform.position;                             //get button on ui pos
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, - Camera.main.transform.position);     //convert z to FOV position 
        Vector3 pos = Camera.main.ScreenToWorldPoint(screenPos);                                        //convert to world pos

        pet.GetComponent<Pets_InMainMenu>().SetCoords(pos);

        //add to list and increment
        pet_list.Add(pet);
    }
}
