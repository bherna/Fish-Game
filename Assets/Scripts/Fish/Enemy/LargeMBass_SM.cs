using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class LargeMBass_SM : Enemy
{

    //Large Mouth Bass _ State Machine
    //This enemy fish will be the first basic type
    //Its purpose is to kill what ever fish it sets its gaze upon
    //its trait is that it ramps up it's speed over time, untill the player clicks on it, restarting its speed

    //there speed is also reset on bite-ing fish

    private float max_velocity = 4; // max velocity bass can reach
    private float curr_velocity = 0; //velocity the bass currently has
    private float acceleration = 0.3f; 

    private float sprite_size = 0.7f;

    private float bounce_vel = 0.4f;

    
    [SerializeField] protected GameObject mouth_obj;

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
            UpdatePos();
        }
        
    }

    private void UpdatePos(){

        //update curr velocity
        if(curr_velocity +Time.deltaTime < max_velocity){ curr_velocity += Time.deltaTime * acceleration; }

        //head towards target 
        var newVector = (currFishTarget.position - transform.position).normalized;
        rb.AddForce(newVector * curr_velocity, ForceMode2D.Force);

        //fliping
        if(transform.position.x - currFishTarget.position.x < 0){
            sprite.localScale = new Vector3(-sprite_size, sprite_size, sprite_size);
            mouth_obj.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else{
            sprite.localScale = new Vector3(sprite_size, sprite_size, sprite_size);
            mouth_obj.transform.localScale = new Vector3(1f, 1f, 1f);    
        }
    }

    public void ResetVelocity(){
        curr_velocity = 0;
    }


    public new void OnPointerClick(PointerEventData eventData) {

        //run original function
        base.OnPointerClick(eventData);

        //reset current built up velocity of fish
        ResetVelocity();
    }


    private void OnTriggerEnter2D(Collider2D other) {

        //if we hit the tank edge
        if(other.gameObject.CompareTag("Boundry")){

            //set our velocity towards middle of tank
            Vector2 newVel = new Vector2(-transform.position.x * bounce_vel, -transform.position.y *bounce_vel);
            rb.velocity = newVel;
        }
    }

}
