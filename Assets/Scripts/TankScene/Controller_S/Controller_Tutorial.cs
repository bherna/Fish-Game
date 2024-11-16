using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Controller_Tutorial : MonoBehaviour
{
    //
    [SerializeField] Mask shop_ui_mask;
    private UI_Dialogue ui_Dialogue;
    public bool tutorial_active = true;


    //tutorial vars
    private int index = 0; //which section of tutorial we are at
    private bool waiting = false; //used in waiting for external event
    private bool[] triggers; //list of all our trigger, once they are true, they should start next tutorial section
    


    //singleton this class
    public static Controller_Tutorial instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }


    
    void Start()
    {
        //make sure we aren't disabled
        //THIS SHOULD ALWAYS BE TRUE (false if we are testing things)
        if (tutorial_active){

            //get reference to ui_dialogue
            ui_Dialogue = transform.GetChild(0).GetComponent<UI_Dialogue>();
           
            //disable shop ui
            shop_ui_mask.enabled = false; //when true it makes it unclickable

            //start dialogue
            //if this returns true (this means we have a tutorial scrip to use)
            if(ui_Dialogue.StartDialogue()){
                Debug.Log("we got tutorial");
                triggers = new bool[4];
                return;
            }
            //else we disable tutorial
        }

        Disable_Tutorial();
    }

    void Update (){

        //if we have our tutorial stil active 
        //keep expecting mouse clicks
        if(Input.GetMouseButtonDown(0) && tutorial_active){
            TutorialClick();
        }
    }


    //Player click method is used to move tutorial forward
    //in each section of tutorial, the player will learn something then wait for some external event to play
    //then next section will play
    public void TutorialClick(){
        Debug.Log("adfs");
        switch(index){
            case 1:
                //first section of the tutorial:
                //welcome player
                //player will learn how to buy first guppy
                //then wait for guppy to get hungry
                if(!waiting){
                    //then we have more words to read through
                    //check if this next click ends the script
                    KeepReading();
                }
                else{
                    //we are done reading and we now wait for our trigger
                    //what external event are we waiting for
                    Waiting();
                }
                break;



            case 2:
                //second section, 
                //now that our fish is hungry, player learns how to feed guppy
                //player feeds guppy
                //wait
                if(!waiting){
                    //then we have more words to read through
                    //check if this next click ends the script
                    KeepReading();
                }
                else{
                    //if this trigger goes off
                    if(Waiting()){
                        //third script is for enemy waves so
                        //start enemy waves
                        Controller_Enemy.instance.StartWaves();
                    }
                    
                }
                break;
    

            case 3:
                //third section
                //player encounters first enemy wave
                //player learns how to get rid of enemies
                if(!waiting){
                    //then we have more words to read through
                    //check if this next click ends the script
                    KeepReading();
                }
                else{
                    //we are done reading and we now wait for our trigger
                    //what external event are we waiting for
                    Waiting();
                }
                break;


            default:
                Debug.Log(string.Format("end of tutorial"));
                Disable_Tutorial();
                break;
        }
        
        
        
    }


    //first half of each tutorial section
    //if we have more strings to print to player, then we run the next
    //else we start waiting
    private void KeepReading(){

        if(!ui_Dialogue.Click()){
            //now we wait for event
            waiting = true;
            //also disable the dialouge ui
            ui_Dialogue.ToggleDialogueBox(false);
        }
        
    }
    //second half of each tutorial section
    //This is the waiting half
    //this function return true once the trigger goes off
    private bool Waiting(){

        if(triggers[index]){
            //event was triggered
            //so we get next script
            index++;
            waiting = false;
            ui_Dialogue.GetJsonScriptNumber(index.ToString());
            return true;
        }
        return false;
    }

    private void Disable_Tutorial(){

        //enable timer
        Controller_Timer.instance.StartTimer();

        //enable ui shop
        shop_ui_mask.enabled = false;

        //start enemy waves
        //since we don't want to start right at game start.
        Controller_Enemy.instance.StartWaves();


        //----- last ----- //
        //disable this object
        //and set tutorial to false
        tutorial_active = false; 
        ui_Dialogue.ToggleDialogueBox(false);
    }



    // ----- types of ways events can trigger ------


    // button trigger, these are triggered from the shop buttons
    //each button is assinged an index 
    //index # == to the obj index in the shop_container list
    public void ShopButtonClick(int index){
        if(!tutorial_active){return;}

        //first index holds the guppy button, so
        if(index == 1){
            triggers[index] = true;
        }
    }


    // enemywave trigger, when ever a new enemy wave starts, this is executed
    public void EnemyWaveStarting(){
        if(!tutorial_active){return;}
        if(index != 3){return;}
        triggers[index] = true;
    }


    //whenever a fish eats food, this is executed
    public void FishHungry(){
        if(!tutorial_active){return;}
        if(index != 2){return;}
        triggers[index] = true;
    }
    
}
