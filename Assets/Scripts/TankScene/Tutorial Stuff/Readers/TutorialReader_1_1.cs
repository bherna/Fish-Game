using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class TutorialReader_1_1 : TutorialReaderParent
{

    //we should only have variables here if
    //they don't account for all tutorial readers
    //soo
    private Vector3 starvePos; //if a tutorial guppy dies, we want to know where they died to spawn money

    //this is specific to this tutorial, so don't bother adding to the parent class



    void Update (){

        //if we have our tutorial stil active 
        //keep expecting mouse clicks
        if(Input.GetMouseButtonDown(0)){
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
                S_Welcome_UnlockGuppy();
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
                //unlock ageing , wait for coin, 
                S_FedGuppy__UnlockGuppyAgeing();
                break;

            case 5:
                // section 5
                //now that guppy is teen and coin dropped
                //wait for player to collect coin
                S_Basic();
                break;

            case 6:
                //section 6
                //now player collected first coin
                //wait for player to collect _ amount of money
                        //this is checked with each tutorial coin collected
                S_Basic();
                break;

            case 7:
                //section 7
                //player has enough money
                //unlock egg peice in shop
                S_Money__UnlockEggShop();
                break;

            case 8:
                // 8th section
                //egg shop is now available
                //wait for player to buy first egg peice
                S_Basic();
                break;

            case 9:
                //9th section
                //player bought egg piece
                //wait for player to buy second peice
                S_Basic();
                break;

            case 10:
                //10th section
                //player bought second peice
                //wait for player to combine egg peices
                S_Basic();
                break;

            case 11:
                //11th section
                //player combines peices
                //wait for post game ui is enabled
                S_Basic();
                break;

            case 12:
                //12th section
                //post game ui is enabled  is up
                //nothing
                S_PostGameUI__EndTutorial();
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
    
    //there is a base class to these, called basic()
    //it's in the parent class
    
    //When the tutorial starts we are disableing the shop buttons,
    //we dont want the player just buying things willy nilly now
    private void S_Welcome_UnlockGuppy(){
        if(!waiting){
                    
            //then we have more words to read through
            //check if this next click ends the script
            if(!KeepReading()){
                //unlock guppy button
                Controller_Tutorial.instance.SetShopItemActive(0, true);
            }
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
                    Instantiate(Resources.Load("Random/GoldenLight") as GameObject, starvePos, Quaternion.identity);
                }
            }
        }
        //else we wait
    }

    //player has feed guppy
    //now unlock ageing
    private void S_FedGuppy__UnlockGuppyAgeing(){
        if(!waiting){
                    
            //then we have more words to read through
            //check if this next click ends the script
            if(!KeepReading()){
                
                Controller_Fish.instance.TutorialEvent_GuppysNowCanAge();
            }
        }
        //else we wait
    }

    private void S_Money__UnlockEggShop(){
        if(!waiting){
                    
            //then we have more words to read through
            //check if this next click ends the script
            if(!KeepReading()){

                //unlock the 3rd item == 2 index
                Controller_Tutorial.instance.SetShopItemActive(2, true);
                //set up an animation so that its more obvious
                //this trigger will be moved somewhere else
                EggNowAvailable();
            }
        }
        //else we wait
    }


    private void S_Null__UnlockEnemies(){

        if(!waiting){
                    
            //then we have more words to read through
            //check if this next click ends the script
            if(!KeepReading()){
                //unlock enemies
                Controller_Enemy.instance.StartWaves();
                
            }
        }
        //else we wait
    }


    //this is the final script message, so we want to disable the tutorial after we finish reading
    private void S_PostGameUI__EndTutorial(){
        if(!waiting){
            //then we have more words to read through
            //check if this next click ends the script
            KeepReading();
        }
        else{
            //and enable postgame ui, since we close it 
            Controller_Objective.instance.ActivatePostEndGameUI();
            
            //we are done reading and so just end this
            Disable_Tutorial();

        }
    }
    





    //below are types of ways events can trigger (external functions)
    //------------------------- ---------------------- ----------------------------- -------------------

    public override void ShopButtonClick(int buttonIndex){

        //the 'first' index holds the guppy button,
        if(buttonIndex == 0){
            TriggerTemplate(1);
        }
        //this is the egg piece button index
        else if(buttonIndex == 2){
            TriggerTemplate(8);
            TriggerTemplate(9); //we buy the second peice, so just do this
        }
    }

    //whenever a guppy gets hungry, this is executed
    public override void GuppyHungry(){
        TriggerTemplate(2);
    }

    //when ever player feeds guppy
    public override void GuppyAte(){
        TriggerTemplate(3);
    }
    //when guppy doesn't eat and starves to death, we want to makes this one keep looping so
    //
    public override void GuppyStarved(Vector2 pos){
        //save the place die
        starvePos = pos;
        TriggerTemplate((3, 1));
    }
    
    //when a guppy poops their first coin (tutorial coin)
    public override void GuppyDropCoin(){
        TriggerTemplate(4);
    }

    //when player picks up tutorial coin
    public override void CollectCoin(){
        TriggerTemplate(5);

        //check if player made enough money to start next trigger
        //we are setting the price to be 120 since player neede money for food aswell
        if(Controller_Wallet.instance.IsAffordable(120))
        TriggerTemplate(6);
    }

    public override void EggNowAvailable(){

        //once the egg shop animation ends, this trigger gets played
        TriggerTemplate(7);
    }

    // egg piece, trigger this one, player was able to successfully able to combine two egg pieces together
    public override void EggPieceCombined(){

        //on succccessfull combination
        TriggerTemplate(10);
    }


    //return triggertemplate bool
    //  - if our trigger succccessfully went through, we return true
    //  - else false on fails
    //this only works on main script, alt scripts don't return anything
    public override bool PostGameUI(){

        //return true if we are in the correct index to trigger in
        return TriggerTemplate(11);
    }

    

    // ----------------------------------------------------------------------------------------
   

}
