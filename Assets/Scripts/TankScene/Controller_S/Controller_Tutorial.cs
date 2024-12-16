
using Unity.VisualScripting;
using UnityEngine;


public class Controller_Tutorial : MonoBehaviour
{
    private UI_Dialogue ui_Dialogue;
    public bool tutorial_active = true;


    //tutorial vars
    public int index {get; private set;}= 1; //which section of tutorial we are at We start at 1, 
                            //since thats how the json files are saved
    public bool waiting {get; private set;}= false; //used in waiting for external event
    private Vector3 starvePos;
    private bool altText;


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
                    
                    //stuff we might need to init, if thats the case
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
                S_FishGotHungry__AltEvent();
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
        //else we wait
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
        //else we wait
    }


    //we need a way to differ from player letting guppy and not
    //so
    private void S_FishGotHungry__AltEvent(){
        if(!waiting){
                    
            //then we have more words to read through
            //check if this next click ends the script
            if(!KeepReading()){

                
                //once we are done reading, we init the OHHHH GOLDEN LIGHT
                //but make sure they acutally killed the guppy
                if(altText){

                    // ie give player money since they need money for guppy
                    //and since they never bought anything else (hopefully) all they get is guppy price 
                    var fileLoc = "Random/";
                    Instantiate(Resources.Load(fileLoc + "GoldenLight") as GameObject, starvePos, Quaternion.identity);
                }
            }
        }
        //else we wait
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
        //else we wait
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
        //else we wait
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


        if(waiting){

            //if we are in the correct INDEX, and we are WAITING for the trigger to go off
            if(i == index){

                //event was triggered
                NextDialogue();

            }
            //if our i is negative
            //this means that we have failed to do what we wanted
            else if(i*-1 == index ){
                
                //ALT event was triggered
                AltDialogue(i);
            }

        }
        else{
            //else we are not  WAITING, 
            //this means player is still reading/has the dialogue box open

            //BUT, now they have the ability to skip the rest of the current section they are on
            //IF they passed a certain part of the tutorial
            if(ui_Dialogue.CanWeSkip()){

                //if we made it to the skip part, 
                //all we want to make sure we do is skip through the current dialouge until we have non left
                //then reset into a next dialouge mode
                if(i == index){
                    while(ui_Dialogue.Click()){} //while more clicks repeat
                    NextDialogue();
                }

                else if(i*-1 == index){
                    while(ui_Dialogue.Click()){}
                    AltDialogue(i);
                }
            }
        }

    }


    //this is a sub function of the trigger template function
    //this is just for read abililty
    //this function sets up the next dialouge for the player
    private void NextDialogue(){

        //so we get next script
        index++;
        ui_Dialogue.GetJsonScriptNumber(index.ToString());
        //also enable the dialouge ui
        ui_Dialogue.ToggleDialogueBox(true);
        //reset waiting to false
        waiting = false;
        altText = false;
    }

    //this is the same as the normal next dialogue function
    //but this one is for alt dialogue options, so we need to get the current index that this can alt into
    private void AltDialogue(int i){

        //so we stay in the same index, but get the script of ALT event which should be negative version of current index
        ui_Dialogue.GetJsonScriptNumber(i.ToString());
        //also enable the dialouge ui
        ui_Dialogue.ToggleDialogueBox(true);
        //reset waiting to false again, since we don't want to lose reading ability
        waiting = false;
        altText = true;
    }   











    //below are types of ways events can trigger (external)
    //------------------------- ---------------------- ----------------------------- -------------------

    // button trigger, these are triggered from the shop buttons
    //each button is assinged an index 
    //index # == to the obj index in the shop_container list
    public void ShopButtonClick(int buttonIndex){
        if(!tutorial_active){return;}

        //the 'first' index holds the guppy button,
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
    public void GuppyStarved(Vector2 pos){
        if(!tutorial_active){return;}
        //save the place die
        starvePos = pos;
        TriggerTemplate(-3);
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
