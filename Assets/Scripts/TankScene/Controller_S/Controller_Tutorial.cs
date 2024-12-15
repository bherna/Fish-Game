using Steamworks;
using UnityEngine;


public class Controller_Tutorial : MonoBehaviour
{
    private UI_Dialogue ui_Dialogue;
    public bool tutorial_active = true;


    //tutorial vars
    public int index {get; private set;}= 1; //which section of tutorial we are at We start at 1, 
                            //since thats how the json files are saved
    public bool waiting {get; private set;}= false; //used in waiting for external event
    private bool trigger; //used in continuing next tutorial script , we need to keep en-dis-ableing it throughout
    


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

            //get reference to ui_dialogue, and enable just incase we disable it in inspector for vision
            ui_Dialogue = transform.GetChild(0).GetComponent<UI_Dialogue>();
            transform.GetChild(0).gameObject.SetActive(true);

            //are we in the tutorial level
            int lvl = int.Parse(LocalLevelVariables.GetLevel());
            if(lvl == 1){

                //start dialogue
                //if this returns true (this means we have a tutorial scrip to use)
                if(ui_Dialogue.StartDialogue()){
                    
                    trigger = false; // the number of cases we have +1 (just set to some number way above that)
                    return;
                }

                //else we disable tutorial
            }

        }
        //we disable down here, because i feel liek it
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
            case 1:
                // section 1: 
                //welcome player
                //player will learn how to buy first guppy
                //then wait for guppy to get hungry
                S_Basic();
                break;

            case 2:
                // 2 section
                //now we have a guppy
                //make player wait for fish to get hungry (make guppy able to get hungry)
                S_BoughtGuppy__UnlockHunger();
                break;

            case 3:
                // section 3, 
                //now that our fish is hungry, player learns how to feed guppy
                //wait for player to feed guppy
                S_Basic();
                break;

            case 4:
                //section 4
                //player fed guppy
                //wait for enemy wave to start
                S_FedGuppy__UnlockEnemyWaves();
                break;

            case 5:
                // section 5
                //player encounters first enemy wave
                //player kills all the enemies
                S_Basic();
                break;

            case 6:
                //section 6
                //now that enemies are dead
                //wait for guppy to grow and drop a coin
                S_EnemyAllDead__UnlockGuppyAgeing();
                break;

            case 7:
                //section 7
                //now that guppy is teen and coin dropped
                //wait for player to collect coin
                S_Basic();
                break;

            case 8:
                // 8th section
                //player collects coin
                //nothing else so we close the tutorial
                S_CollectCoin__EndTutorial();
                break;


            default:
                Debug.Log(string.Format("We are in a case that doesn't exsist."));
                break;
        }
        
        
        
    }


    //these are all the different switch cases we can be,
    //were sepearating them into here, just for read-ability in the switch function
    //its starting to get mess-i in there
    //-------------------------------------------------------------------------------------------------

    //basic switch function
    //this is essesntially what each switch case does, 
    //we check if we keep reading
    //          else we wait for the expected trigger
    private void S_Basic(){

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
    }


    //this is the first special switch case we have'
    //we use this when player first buys guppy
    //this case then lets guppys get hungry
    //keep reading is also a bool, so we just use it as a check here to start expecting trigger
    private void S_BoughtGuppy__UnlockHunger(){
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
    }



    //player has feed guppy
    //and now we wait for enemy wave to start, so we init it to start
    private void S_FedGuppy__UnlockEnemyWaves(){
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
    }


    //once player eliminates ;[] all the enemies
    //we now let TUTorial guppys age
    private void S_EnemyAllDead__UnlockGuppyAgeing(){
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
    }



    //this is the final script message, so we want to disable the tutorial after we finish reading
    private void S_CollectCoin__EndTutorial(){
        if(!waiting){
            //then we have more words to read through
            //check if this next click ends the script
            KeepReading();
        }
        else{
            //we are done reading and so just end this
            Disable_Tutorial();
        }
    }
    //-------------------------------------------------------------------------------------------------









    //first half of each tutorial section
    //run the click function in the dialogue script
    //if clicking returns that we have more lines to go we keep reading (true)
    //else we read our last line and so we are now ready to wait for our event (false)
    private bool KeepReading(){
        
        //if no more clicks
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

        if(trigger){
            
            //event was triggered
            //so we get next script
            index++;
            ui_Dialogue.GetJsonScriptNumber(index.ToString());
            //also enable the dialouge ui
            ui_Dialogue.ToggleDialogueBox(true);
            //reset waiting to false
            waiting = false;
            trigger = false; //reset to false, since we don't want to repeat this
            return true;
        }
        
        return false;
    }


    private void Disable_Tutorial(){

        //enable timer
        Controller_Timer.instance.StartTimer();

        //start enemy waves incase we didn't start them 
        Controller_Enemy.instance.StartWaves();


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
            trigger = true;
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
    //when guppy doesn't eat and starves to death, we want to makes this one keep looping so
    //
    public void GuppyStarved(){
        if(!tutorial_active){return;}
        
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

    //when a guppy poops their first coin (tutorial coin)
    public void GuppyDropCoin(){
        if(!tutorial_active){return;}
        TriggerTemplate(6);
    }

    //when player picks up tutorial coin
    public void CollectCoin(){
        if(!tutorial_active){return;}
        TriggerTemplate(7);
    }

    

    // ----------------------------------------------------------------------------------------
   


}
