using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject slidePanelsParentObj;         //gameobject reference for all ui level panels + main menu
    [SerializeField] EventOnHover_PlayButton playButton; //used in turning the tank lights on


    private string tankSceneName; //current tank level selected (name)
    private int curr_screen = 0; //main menu == 0, levels, pet panel == -1
    private WorldLevelTuple worldLevel; //used for getting world + level in ints,  currently selected


    //singleton this
    public static Controller_MainMenu instance { get; private set; }
    void Awake()
    {

        //delete duplicate of this instance

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        //set ui tabs positions 
        for (int i = 0; i < slidePanelsParentObj.transform.childCount; i++)
        {
            slidePanelsParentObj.transform.GetChild(i).gameObject.transform.localPosition = new Vector3(Screen.width * i, 0, 0); //set their pos
            slidePanelsParentObj.transform.GetChild(i).gameObject.SetActive(true);
        }

        //set pets panel false, since we dont want to see it
        //also avoid moving it since our pet coords are saved from this (0,0) position
        transform.Find("Pets - Container").gameObject.SetActive(false);

        //set our discord activity to in main menu
        //DiscordManager.instance.ChangeActivity(DiscordManager.DiscordState.Menu, 0, 0);
    }


    //Go to scene currSceneSet
    public void PlayLevel()
    {

        //save selected pets
        PetsAccess.UpdateSetSelectedPets(Controller_PetMenu.instance.selectedPets);

        //set our current level in discord
        //DiscordManager.instance.ChangeActivity(DiscordManager.DiscordState.Level, worldLevel.Item1, worldLevel.Item2);

        //start level last
        SceneManager.LoadScene(tankSceneName);
    }


    public void NewGame()
    {

        //reset our save files
        LevelsAccess.NewGame();
        PetsAccess.NewGame();

        //reset our level selection 
        Controller_LevelMenu.instance.UI_LevelsUpdateAccess();

        //reset our pets in main menu
        Controller_PetMenu.instance.ResetPets();

    }

    //save game button reference
    public void SaveGame()
    {
        SaveLoad.SaveGame();
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("application has quit.");
    }

    public void Next_UIScreen()
    {

        for (int i = 0; i < slidePanelsParentObj.transform.childCount; i++)
        {
            slidePanelsParentObj.transform.GetChild(i).gameObject.transform.localPosition -= new Vector3(Screen.width, 0, 0);
        }

        curr_screen += 1;
    }

    public void Previous_UIScreen()
    {

        for (int i = 0; i < slidePanelsParentObj.transform.childCount; i++)
        {
            slidePanelsParentObj.transform.GetChild(i).gameObject.transform.localPosition += new Vector3(Screen.width, 0, 0);
        }

        curr_screen -= 1;

        //logic to reset the play button to turn of the lights in the fish tank
        if (curr_screen == 0)
        {
            playButton.OnPointerReturnToTitleScreen();
        }
    }


    //set scene currSceneSet
    //this lets us move from level select to pet select
    public void GoToPetsPanel(string tankScene, WorldLevelTuple level)
    {

        //save current tank scene name, so we go to that level after selecting pets
        tankSceneName = tankScene;
        worldLevel = level;
        //update the level name aswell, used in the controller enemy script for choosing what json enemywaves file
        LocalLevelVariables.UpdateLevel(level);

        //---move to last panell---//
        //disbale current level panel
        //eneable pet panel
        slidePanelsParentObj.transform.GetChild(curr_screen).gameObject.SetActive(false);
        transform.Find("Pets - Container").gameObject.SetActive(true);


        //---have pets move to corresponding ui button location---//
        Controller_PetMenu.instance.PetsToButton();

        //---update selected pets in ui screen---//
        //this should be done in the pet_togglebutton script
    }


    //if player want to leave pet selection -> return to level selection, return to world panel they were last on
    public void ReturnToLevelSelection()
    {

        //return to last level panel player was on before
        //enable level panel
        //disable pet panel
        slidePanelsParentObj.transform.GetChild(curr_screen).gameObject.SetActive(true);
        transform.Find("Pets - Container").gameObject.SetActive(false);

        //let pets return to idle
        Controller_PetMenu.instance.PetsToIdle();

        //update selected pets
        PetsAccess.UpdateSetSelectedPets(Controller_PetMenu.instance.selectedPets);

    }



    public void PlayTestingScene()
    {
        //start level last
        SceneManager.LoadScene("TestingScene");
    }
}
