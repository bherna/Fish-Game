using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialReader_1_2 : TutorialReaderParent
{
    
    
    //im cheating by doing this but
    //just used OnTutorialStart to skip the first dialogue, (its empty here anyways,)
    //just run an artifial click to close it / enter waiting mode instantly
    protected override void OnTutorialStart(){

        //ie: update on click down
        TutorialState();
    }


    //reference tutorail reader 1-1 for description on what this does
    protected override void TutorialState(){



        switch(index){

            case 1:
                //this is a null index, we are using it just to wait
                //wait for enemy wave to be annouced
                S_Null__UnlockEnemies();
                break;
            case 2:
                //first enemy wave annoouced, player will learn about enemies
                //then wait for player to finish enemy wave off
                S_Basic();
                break;
                
            case 3:
                //now that enemy wave is over, Tell player good job!
                //end tutorail, (they don't need to learn about combos or counters here, maybe tank 2 stuff)
                S_EnemyWaveOver__EndTutorial();
                break;


        }
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


    private void S_EnemyWaveOver__EndTutorial(){
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

    




    /// --------------------------------------------------------------- triggers for this reader down bellow ---------------------------------------------------------------

    public override void EnemyWaveStarting(){

        //trigger is called from controller enemy, once enemy wave is called
        TriggerTemplate(1);
    }

    public override void EnemyWaveOver(){

        //trigger is called from controller enemy, once llast enemy is killed
        TriggerTemplate(2);
    }









}
