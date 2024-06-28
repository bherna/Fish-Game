using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeMBass_SM : Enemy
{
    private int damageAmount = 20; 




    void Update()
    {
        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if(Controller_Main.instance.paused){
            return;
        }

        //if no fish currently pointing to
        if(currFishTarget == null){
            
            //if so get a new fish to follow
            SetTargetFish(Controller_Fish.instance.GetRandomFish());

            //if we can't find a fish, just return
            if(currFishTarget == null){

                //wait a few seconds before checking again ?
                return;
                
            }
            
        }

        else{
            //update the enemy position towards target fish
            updatePos();
        }
        
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Fish")){
            
            other.gameObject.GetComponent<Fish_Stats>().TakeDamage(damageAmount);


        }
    }



}
