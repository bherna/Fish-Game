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
//Fish_Hungry:  expects player to feed fish     ->
public enum Expect_Type {TextBox, Button, Fish_Hungry};


public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] Timer timer;
    [SerializeField] Mask shop_ui_mask;



    private UI_Dialogue uI_Dialogue;

  
    public bool tutorial_active = true;


    //static variable for fish coin value
    public static UI_Tutorial instance {get; private set; }
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
        uI_Dialogue = GetComponent<UI_Dialogue>();

        //disable shop ui
        shop_ui_mask.enabled = true; //when true it makes it unclickable

        //start dialogue
        uI_Dialogue.StartDialogue();

        //if we need the mask for shop items off, we would check here
        //ui_dialogue.curr_expectTYpe == button ....
    }

    void Update (){

        if(Input.GetMouseButtonDown(0)){
            Playerclick(Expect_Type.TextBox);
        }
    }


    //Player click method is used to move tutorial forward
    //method expects an eventR expect_type (where this method was called from)(from button or from this class)
    public void Playerclick(Expect_Type eventR){

        //did the player click the correct thing to push tutorial
        if(uI_Dialogue.curr_expectType != eventR){
            Debug.Log("Incorrect click for tutorial");
            return;
        }

        //if player clicks
        //and its true -> we have more lines to go
        if(uI_Dialogue.Click()){

            //for every new click
            //disable shop
            shop_ui_mask.enabled = true; //re enable in expect type
            uI_Dialogue.ToggleDialogueBox(true); // re enable in expect type

            //get new/current dia expect
            var type = uI_Dialogue.curr_expectType;

            //
            switch(type){

                //get next line's curr_expectType, check if we need buttons active
                case Expect_Type.Button:
                    //ENABLE ui shop (mask being on makes buttons unresponsive)
                    shop_ui_mask.enabled = false;
                    break;

                case Expect_Type.Fish_Hungry:
                    //diable dialogue box, since we don't need to show it for now
                    uI_Dialogue.ToggleDialogueBox(false);
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
            //enable timer
            timer.StartTimer();

            //enable ui shop
            shop_ui_mask.enabled = false;

            //disable this object
            //and set tutorial to false
            tutorial_active = false; 
            gameObject.SetActive(false);
        }
        
    }

    
}
