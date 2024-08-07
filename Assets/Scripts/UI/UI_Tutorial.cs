using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Expect type is used to tell what we expect the next click to be from player to 
//progress the tutorial steps
public enum Expect_Type {TextBox, Button};


public class UI_Tutorial : MonoBehaviour
{
    [SerializeField] Timer timer;
    [SerializeField] Mask shop_ui_mask;



    private UI_Dialogue uI_Dialogue;

  
    


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

        }
        //else we are finished with tutorial and we can start game.
        else{
            //enable timer
            timer.StartTimer();

            //enable ui shop
            shop_ui_mask.enabled = false;

            //disable this object
            gameObject.SetActive(false);
        }
        
    }

    
    
}
