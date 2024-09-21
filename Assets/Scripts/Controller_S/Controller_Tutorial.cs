using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Expect type is used to tell what we expect the next click to be from player to ->
//progress the tutorial steps
//
//Types:
//TextBox:      expects player to click textbox ->
//Button:       expects player to click button  ->
//Wait:         makes player wait until next tutorial message -> (event needs to cause this one to push) 
//fish_Hungry:  push next tutorial message      ->
//Fish_Feed:    expects player to feed fish     ->
public enum Expect_Type {TextBox, Button, Wait, Fish_Hungry, Fish_Feed};


public class Controller_Tutorial : MonoBehaviour
{
    [SerializeField] Timer timer;
    [SerializeField] Mask shop_ui_mask;



    [SerializeField] UI_Dialogue uI_Dialogue;

  
    public bool tutorial_active = true;
    


    //static variable for fish coin value
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
        if (tutorial_active){
           
            //disable shop ui
            shop_ui_mask.enabled = true; //when true it makes it unclickable

            //start dialogue
            uI_Dialogue.StartDialogue();

            //if we need the mask for shop items off, we would check here
            //ui_dialogue.curr_expectTYpe == button ....
        }
        else{
            Disable_Tutorial();
            Debug.Log("Tutorial is set to - disabled - this round.");
        }

    }

    void Update (){

        //click in tank to 'click' text box
        if(Input.GetMouseButtonDown(0) && tutorial_active){
            TutorialClick(Expect_Type.TextBox);
        }
    }


    //Player click method is used to move tutorial forward
    //method expects an eventR expect_type (where this method was called from)(from button or from this class)
    public void TutorialClick(Expect_Type eventR){

        //did the player click the correct thing to push tutorial
        if(uI_Dialogue.curr_expectType != eventR){
            //Debug.Log("Incorrect click for tutorial: uI_Dialogue.curr_expectType");
            return;
        }

        //if player clicks
        //and its true -> we have more lines to go
        if(uI_Dialogue.Click()){

            //for every new click
            //disable shop
            shop_ui_mask.enabled = true; //re enable in expect type

            //get new/current dia expect
            var type = uI_Dialogue.curr_expectType;

            //Is there anything this up-comming expect_Type event needs help with to be pushed out
            switch(type){

                //get next line's curr_expectType, check if we need buttons active
                case Expect_Type.Button:
                    //ENABLE ui shop (mask being on makes buttons unresponsive)
                    shop_ui_mask.enabled = false;
                    break;

                case Expect_Type.Wait:
                    //diable dialogue box, since we don't need to show it for now
                    //(next expect_type might not nessasarily need to be active)
                    // ex (fish_hungry will not be displayed but fish_feed will)
                    uI_Dialogue.ToggleDialogueBox(false);
                    //move to next line
                    TutorialClick(Expect_Type.Wait);
                    break;

                case Expect_Type.Fish_Hungry:
                    break;

                case Expect_Type.Fish_Feed:
                    //re-enable dialoge box
                    uI_Dialogue.ToggleDialogueBox(true); 
                    break;

                case Expect_Type.TextBox:
                    break;
                
                default:
                    Debug.Log("No expect type set.");
                    break;
            }
            
            

        }
        //else we are finished with tutorial and we can start game.
        else{
            Disable_Tutorial();
        }
        
    }

    private void Disable_Tutorial(){
        //enable timer
        timer.StartTimer();

        //enable ui shop
        shop_ui_mask.enabled = false;

        //disable this object
        //and set tutorial to false
        tutorial_active = false; 
        uI_Dialogue.ToggleDialogueBox(false);
    }
}
