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
    public int index {get; private set;}= 1; //which section of tutorial we are at We start at 1, 
                            //since thats how the json files are saved
    public bool waiting {get; private set;}= false; //used in waiting for external event
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
                
                triggers = new bool[20]; // the number of cases we have +1 (just set to some number way above that)
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
            TutorialState();
        }
    }


    //Player click method is used to move tutorial forward
    //in each section of tutorial, the player will learn something then wait for some external event to play
    //then next section will play
    public void TutorialState(){
        
        switch(index){
            case 1 or 3 or 5 or 7:
                // section 1:
                //welcome player
                //player will learn how to buy first guppy
                //then wait for guppy to get hungry

                // section 3, 
                //now that our fish is hungry, player learns how to feed guppy
                //wait for player to feed guppy

                // section 5
                //player encounters first enemy wave
                //player lkills all the enemies

                //section 7
                //now that guppy is teen and coin dropped
                //wait for player to collect coin
                if(!waiting){
                    //then we have more words to read through
                    //check if this next click ends the script
                    KeepReading();
                }
                else{
                    //we are done reading and we now wait for our trigger
                    //what external event are we waiting for
                    WaitingForTrigger();
                }
                break;


            case 2:
                // 2 section
                //now we have a guppy
                //make player wait for fish to get hungry (make guppy able to get hungry)
                if(!waiting){
                    
                    //then we have more words to read through
                    //check if this next click ends the script
                    if(!KeepReading()){
                        //let guppy get hungry
                        Controller_Fish.instance.TutorialEvent_GuppysNowCanEat();
                    }
                }
                else{
                    //wait for enemy wave to start
                    WaitingForTrigger();
                }
                break;

            case 4:
                //section 4
                //player fed guppy
                //wait for enemy wave to start
                if(!waiting){
                    
                    //then we have more words to read through
                    //check if this next click ends the script
                    if(!KeepReading()){
                        //start the enemy waves 
                        Controller_Enemy.instance.StartWaves();
                    }
                }
                else{
                    //wait for enemy wave to start
                    WaitingForTrigger();
                }
                break;

            case 6:
                //section 6
                //now that enemies are dead
                //wait for guppy to grow and drop a coin
                if(!waiting){
                    
                    //then we have more words to read through
                    //check if this next click ends the script
                    if(!KeepReading()){
                        //start the enemy waves 
                        Controller_Fish.instance.TutorialEvent_GuppysNowCanAge();
                    }
                }
                else{
                    //wait for enemy wave to start
                    WaitingForTrigger();
                }
                break;

            case 8:
                // 8th section
                //player collects coin
                //nothing else so we close the tutorial
                if(!waiting){
                    //then we have more words to read through
                    //check if this next click ends the script
                    KeepReading();
                }
                else{
                    //we are done reading and so just end this
                    Disable_Tutorial();
                }
                break;


            default:
                Debug.Log(string.Format("We are in a case that doesn't exsist."));
                break;
        }
        
        
        
    }


    //first half of each tutorial section
    //run the click function in the dialogue script
    //if clicking returns that we have more lines to go we keep reading (true)
    //else we read our last line and so we are now ready to wait for our event (false)
    private bool KeepReading(){
        
        if(!ui_Dialogue.Click()){
            
            //now we wait for event
            waiting = true;
            //also disable the dialouge ui
            ui_Dialogue.ToggleDialogueBox(false);

            return false;
        }
        
        return true;
    }
    //second half of each tutorial section
    //This is the waiting half
    //this function return true once the trigger goes off
    private bool WaitingForTrigger(){

        if(triggers.Length < index){
            Debug.Log(string.Format("our tutorial trigger array is to small (pls make bigger)."));
            return false;
        }

        if(triggers[index]){
            
            //event was triggered
            //so we get next script
            index++;
            ui_Dialogue.GetJsonScriptNumber(index.ToString());
            //also enable the dialouge ui
            ui_Dialogue.ToggleDialogueBox(true);
            //reset waiting to false
            waiting = false;
            return true;
        }
        
        return false;
    }




    private void Disable_Tutorial(){

        //enable timer
        Controller_Timer.instance.StartTimer();

        //enable ui shop
        shop_ui_mask.enabled = false;


        //----- last ----- //
        //disable this object
        //and set tutorial to false
        tutorial_active = false; 
        ui_Dialogue.ToggleDialogueBox(false);
    }



    
    // -------------------------  ------------------------------------
    //this function is set into each of the triggers
    //i = what index we are supposed to be in, (this is pretty much the json file # we are using, its also our case #)
    //if our i doesn't match our index, then we dont have the correct trigger
    private void TriggerTemplate(int i){
        //if we are in the correct INDEX, and we are WAITING for the trigger to go off
        if(index == i && waiting){
            //if we have a match then we should set trigger to true
            triggers[index] = true;
            //then run the tutorial function to continue
            TutorialState();
        }
    }




    //below are types of ways events can trigger (external)
    //------------------------- ---------------------- ----------------------------- -------------------

    // button trigger, these are triggered from the shop buttons
    //each button is assinged an index 
    //index # == to the obj index in the shop_container list
    public void ShopButtonClick(int buttonIndex){
        if(!tutorial_active){return;}
        //first index holds the guppy button, so
        if(buttonIndex == 1){
            TriggerTemplate(1);
        }
    }

    //whenever a guppy gets hungry, this is executed
    public void GuppyHungry(){
        if(!tutorial_active){return;}
        TriggerTemplate(2);
    }

    //when ever player feeds guppy
    public void GuppyAte(){
        if(!tutorial_active){return;}
        TriggerTemplate(3);
    }
    
    // enemywave trigger, when ever a new enemy wave starts, this is executed
    public void EnemyWaveStarting(){
        if(!tutorial_active){return;}
        TriggerTemplate(4);
    }

    //once the last enemy is killed this function plays
    public void EnemyWaveOver(){
        if(!tutorial_active){return;}
        TriggerTemplate(5);
    }

    public void GuppyDropCoin(){
        if(!tutorial_active){return;}
        TriggerTemplate(6);
    }

    public void CollectCoin(){
        if(!tutorial_active){return;}
        TriggerTemplate(7);
    }

    
}
