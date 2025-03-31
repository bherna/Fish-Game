using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_EscMenu : MonoBehaviour
{

    //Here we have paused and eseMenuOpen, 
    //when the game is paused, the tank's delta time is set to 0
    //but that doesn't mean that the esc menu is open, we can have it open without DIRECTLY pausing
    //but having the esc menu open SHOULD HAVE the tank paused.
    //so
    //if the tank is not pause and we open the esc menu, then we pause here
    //if the tank is alreadyyy paused and we open the esc menu, then we don't bother pausing, but we flag this
    
    public bool paused {get; private set;} = false;
    private bool escMenuOpen = false;


    //the flag bool is used to tell this esc menu class to either unpause the tank when we close the menu or not
    //since we can have an _ number of pauses going on, we count total pauses
    // when we pause -> +1 flag, unpause -> -1 flag (then we check if flag == 0, causeing unpause tank)
    private int flag = 0; //


    //references to the shop ui and the esc menu
    //shop ui so player can't buy when menu open
    //and esc menu reference to open and close
    [SerializeField] GameObject Shop_Container;
    [SerializeField] GameObject Esc_UI;


    //a list of the current interactive setting for each button in the shop
    //if we dont know what they were before disableing them, then
    //we don't know if they should be turned on yet, 
    //since we have progression now, we don't want to just turn them all on (CHEATERS)
    private bool[] interactive_list;


    //singleton this class
    public static Controller_EscMenu instance {get; private set; }
    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }   




    private void Update() {
        

        //escape - open menu
        if(Input.GetKeyUp(KeyCode.Escape) && !escMenuOpen){
            
            //pause game
            //open escape menu
            OpenMainMenu();
            escMenuOpen = true;


        }
        else if(Input.GetKeyUp(KeyCode.Escape) && escMenuOpen) {

            //unpause game
            //close escape menu
            CloseMainMenu();
            escMenuOpen = false;


        }
    }


//-----------------------------------------------------------------------------------------------------------
//this functions are used for opening and closing the main menu ui stuff
    public void OpenMainMenu(){

        //pause all time references (physics, time)
        PauseTank(true);
        //pause audio listeners
        AudioListener.pause = true;


        //disable ui buttons (so player can't purchase)
        interactive_list = new bool[Shop_Container.transform.childCount];
        int i = 0;
        //                                                          the true here is on purpose, we want to grab all buttons,
        //                                                          not just the active ones
        foreach(var btn in Shop_Container.GetComponentsInChildren<Button>(true)){

            interactive_list[i] = btn.interactable;//save
            i++;
            btn.interactable = false; //then disable

        }

        //enable esc ui
        Esc_UI.SetActive(true);

        //if tutorial is active, hide from view
        //make sure it exists first
        if(Controller_Tutorial.instance.tutorial_active){
            TutorialReaderParent.instance.HideTutorial(false);
        }
    }

    public void CloseMainMenu(){

        PauseTank(false);
        AudioListener.pause = false;

        int i = 0;
        foreach(var btn in Shop_Container.GetComponentsInChildren<Button>(true)){
            btn.interactable = interactive_list[i];
            i++;
        }

        //disable esc ui
        Esc_UI.SetActive(false);

        //if tutorial is active, return its view
        TutorialReaderParent.instance.HideTutorial(true);
    }


//-----------------------------------------------------------------------------------------------------------

    




    //this function is used in making sure our tank isn't already paused
    //if it is, we don't want to unpause after we close the esc menu
    //it can also be referenced by other functions, outside this obj
    //parameter: true == pause the tank, false == unpause
    public void PauseTank(bool pauseOrNot){

        
        if(pauseOrNot){
            //just add one to flag
            flag++;
            //since we don't know if this is the first or not
            //we just set to 0, doesn't hurt
            Time.timeScale = 0; 
            //now we are offisshally paused
            paused = true;
        }
        else{
            //we want to unpause
            //so
            flag--; //sub 1
            //then check for 0
            if(flag <= 0){
                //unpause
                Time.timeScale = 1;
                //yea
                paused = false;
            }
        }
    }
}
