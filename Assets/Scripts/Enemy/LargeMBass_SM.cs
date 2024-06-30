using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class LargeMBass_SM : Enemy
{

    //Large Mouth Bass _ State Machine
    //This enemy fish will be the first basic type
    //Its purpose is to kill what ever fish it sets its gaze upon
    //its trait is that it ramps up it's speed over time, untill the player clicks on it, restarting its speed

    private float curr_velocity = 0;
    private float acceleration = 0.2f;


    //bite-ing fish vars
    private int attackPower = 20; 
    private int attackSpeed = 1; //per second
    private bool canAttack = true;



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

    private new void updatePos(){

        //update curr velocity
        if(curr_velocity +Time.deltaTime < velocity){ curr_velocity += Time.deltaTime * acceleration; }

        //head towards target 
        var newVector = (currFishTarget.position - transform.position).normalized;
        rb.AddForce(newVector * curr_velocity, ForceMode2D.Force);

        //sprite fliping
        if(transform.position.x - currFishTarget.position.x < 0){
            sprite.localScale = new Vector3(-1f, 1f, 1f);
        }
        else{
            sprite.localScale = new Vector3(1f, 1f, 1f);
        }
    }



    private void OnTriggerStay2D(Collider2D other) {
        
        if(canAttack && other.gameObject.CompareTag("Fish")){
            
            //bite
            other.gameObject.GetComponent<Fish_Stats>().TakeDamage(attackPower);

            //now wait for next bite
            canAttack = false;
            IEnumerator co = AttackCooldown();
            StartCoroutine(co);

        }
    }

    private IEnumerator AttackCooldown() {

        yield return new WaitForSeconds(1/attackSpeed);
        canAttack = true;
    }

    public new void OnPointerClick(PointerEventData eventData) {

        base.OnPointerClick(eventData);

        //reset current built up velocity of fish
        curr_velocity = 0;
    }

}
