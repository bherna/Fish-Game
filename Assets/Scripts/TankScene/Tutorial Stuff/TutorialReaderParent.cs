using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialReaderParent : MonoBehaviour
{

    //singleton this class
    public static TutorialReaderParent instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }


    //tutorial vars
    public int index {get; private set;}= 1; //which section of tutorial we are at We start at 1, 
                            //since thats how the json files are saved
    public bool waiting {get; private set;}= false; //we are either WAITING for triggers or displaying dialogue
    protected bool altText; //if true, we enter a alternative dialogue option
    protected bool unpauseFlag = false; //if true, then we should tell the escmenu controller we should unpause the tank
    
    protected UI_Dialogue ui_Dialogue; //yea we set it again, because Idk
    

    void Start()
    {
    
        
        //get reference to ui_dialogue,
        ui_Dialogue = Controller_Tutorial.instance.GetUI_Dialogue();

        //are we in the tutorial level
        if(LocalLevelVariables.ThereIsTutorial()){

            //enable just incase we disable it in inspector for vision
            Controller_Tutorial.instance.SetUIActive(true);

            //start dialogue
            //if this returns true (this means we have a tutorial scrip to use)
            if(ui_Dialogue.StartDialogue()){
                
                //-----------------------------------------------  stuff we might need to init, if thats the case -----------------------------------------------

                //like the shop buttons, disable thems shts
                for(int i = 0; i < Controller_Tutorial.instance.GetShopItemsCount(); i++){
                    Controller_Tutorial.instance.SetShopItemActive(i, false);
                }
                
            }
        }
        //else we disable tutorial
        else{
            Disable_Tutorial();
        }
        
    }


    //-------------------------------------------------------------------------------------------------
    //basic switch function
    //this is essesntially what each switch case does, 
    //we check if we keep reading
    //          else we wait for the expected trigger
    protected void S_Basic(){

        if(!waiting){
            //then we have more words to read through
            //check if this next click ends the script
            KeepReading();
        }
        //else we wait
    }


    //first half of each tutorial section
    //run the click function in the dialogue script
    //if clicking returns that we have more lines to go we keep reading (true)
    //else we read our last line and so we are now ready to wait for our event (false)
    protected bool KeepReading(){
        
        //if no more clicks
        if(!ui_Dialogue.Click()){
            
            //now we wait for event
            waiting = true;
            //also disable the dialouge ui
            ui_Dialogue.ToggleDialogueBox(false);

            //unpause the tank, if needed
            if(unpauseFlag){
                Controller_EscMenu.instance.PauseTank(false);
                unpauseFlag = false;
            }

            return false;
        }
        
        return true;
    }



    //these are ust post tutorial things we want to turn on
    //i dont' really have a better place to put these so...
    protected void Disable_Tutorial(){

        //enable timer
        Controller_Timer.instance.StartTimer();

        //start enemy waves incase we didn't start them 
        Controller_Enemy.instance.StartWaves();


        //----- last ----- //
        //disable this object
        //and set tutorial to false 
        Controller_Tutorial.instance.DisableTutorial();
        ui_Dialogue.ToggleDialogueBox(false);
        gameObject.SetActive(false);
    }




    // -------------------------  ------------------------------------
    //this function is set into each of the triggers
    //i = what index we are supposed to be in, (this is pretty much the json file # we are using, its also our case #)
    //if our i doesn't match our index, then we dont have the correct trigger

    //we also return bool which checcks if we succcessfully triggerd (are waiting and correct index)
    protected bool TriggerTemplate(int i){

        if(waiting){

            //if we are in the correct INDEX, and we are WAITING for the trigger to go off
            if(i == index){

                //event was triggered
                NextDialogue();
                return true;
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
                return true;
            }
        }
        
        //if we wern't  successfull, return false then
        return false;
    }

    //same logic as above, just that we are checking for alternative scripts, 
    //since there can be multiple alternantives, we need a way to differ from each one,
    // ( on an int is not possible, so we use a tuple)
    //tuple (1, 2), 
    // 1 == index we want to be in
    // 2 == which alternative script we want to run off this, if correct index, alternatives start at 1
    protected void TriggerTemplate((int, int) i){

        if(waiting){

            //if we are in the correct INDEX, and we are WAITING for the trigger to go off
            //since we are a tuple, we were set off as an alternative trigger
            //this means that we have failed to do what we wanted
            //and so check if we are in the correct index first
            if(i.Item1 == index ){
                
                //ALT event was triggered
                AltDialogue(i.Item2);
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
                if(i.Item1 == index){
                    while(ui_Dialogue.Click()){}
                    AltDialogue(i.Item2);
                }
            }
        }
    }

    
    //this is a sub function of the trigger template function
    //this is just for read-abililty
    //this function sets up the next dialouge for the player
    protected void NextDialogue(){

        //so we get next script
        index++;
        ui_Dialogue.GetJsonScriptNumber(index, 0);
        //also enable the dialouge ui
        ui_Dialogue.ToggleDialogueBox(true);
        //reset waiting to false
        waiting = false;
        altText = false;

        //also check if we should pause the tank
        PauseTank();
    }

    //this is the same as the normal next dialogue function
    //but this one is for alt dialogue options, so we need to get the current index that this can alt into
    protected void AltDialogue(int alt_i){

        //so we stay in the same index, but get the ALT script same index, just differ alt index
        ui_Dialogue.GetJsonScriptNumber(index, alt_i);
        //also enable the dialouge ui
        ui_Dialogue.ToggleDialogueBox(true);
        //reset waiting to false again, since we don't want to lose reading ability
        waiting = false;
        altText = true;

        //also check if we should pause the tank
        PauseTank();
    }   


    //since we dont want to be willy nilly saying to the esc menu that we should unpause
    //the tank whenever we feel like it
    //we have to check for false triggers
    protected void PauseTank(){

        //only if true
        if( ui_Dialogue.pause){
            Controller_EscMenu.instance.PauseTank(true);
            //and set flag
            unpauseFlag = true;
        }
    }


    //used in the esc menu
    //when we need to pause the game
    //when toggle == true, we set dialogue to visable
    public void HideTutorial(bool toggle){
        ui_Dialogue.ToggleDialogueBox(toggle);
    }

    












    //below are types of ways events can trigger (external functions)
    //------------------------- ---------------------- ----------------------------- -------------------

    //Generalized ones go first ---------------------------------------------------------------------
    // button trigger, these are triggered from the shop buttons
    //each button is assinged an index 
    //index # == to the obj index in the shop_container list
    public virtual void ShopButtonClick(int buttonIndex){}




    //Level 1-1 triggers:--------------------------------------------------------------------------------------
    public virtual void GuppyHungry(){}
    public virtual void GuppyAte(){}
    public virtual void GuppyStarved(Vector2 pos){}
    public virtual void GuppyDropCoin(){}
    public virtual void CollectCoin(){}
    public virtual void EggNowAvailable(){}
    public virtual void EggPieceCombined(){}
    public virtual bool PostGameUI(){return false;}



    //Level 1-3 triggers: ------------------------------------------------------------------------------------
    public virtual void EnemyWaveStarting(){}
    public virtual void EnemyWaveOver(){}
}
