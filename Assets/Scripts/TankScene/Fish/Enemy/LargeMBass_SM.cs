using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LargeMBass_SM : Enemy_ParentClass
{

    //Large Mouth Bass _ State Machine
    //This enemy fish will be the first basic type
    //Its purpose is to kill what ever fish it sets its gaze upon
    //its trait is that it ramps up it's speed over time, untill the player clicks on it, restarting its speed



    //how this script will work
    //lm-bass SM script job is to decide which state we should be in
    //if in idle mode, we just roam around the tank
    //if attack mode, head towards target, once target in range -> bite
    //on update, add momentum
    //if player clicks lm-bass: reset momentum
    //if lm-bass reaches edge of tankk: push towards center of tank

    //references
    [SerializeField] ParticleSystem bite_particle;

    //movement related
    private const float max_velocity = 4; // max velocity bass can reach
    private float curr_velocity = 0; //velocity the bass currently has
    private const float acceleration = 0.3f; //how fast lm-bass builds up speed
    private const float bounce_vel = 0.4f;  //kb force 


    //attack related
    private const float attack_range = 1f; //how far out the mouth of lm-bass can go from body base
    private const float attackSpeed = 3f; // _ seconds we need to wait until we can attack again
    private float attackSecs = 0f;
    private const int attackPower = 3; //attack power is in terms of bites (attack power = _ bites worth of damage) 

        

    private new void Update()
    {
        base.Update();

        switch(curr_EnemyState){

            case Enemy_States.idle:
                IdleMode();
                UpdateCooldown();
                break;
            case Enemy_States.attack:
                AttackMode();
                break;
            default:
                Debug.Log(gameObject+" does not have a state to run.");
                break;
        }
    }


    //used for updating our cooldown setting
    private void UpdateCooldown(){

        //check if we should switch to attack mode
        attackSecs += Time.deltaTime;
        if(attackSecs >= attackSpeed){
            attackSecs = 0;
            curr_EnemyState = Enemy_States.attack;
        }
    }

    private void AttackMode(){

        //if no fish currently pointing to
        if(currFishTarget == null){
            
            //if so get a new fish to follow
            SetTargetFish(Controller_Fish.instance.GetRandomFish());

            //if we can't find a fish, run idle mode
            if(currFishTarget == null){
                curr_EnemyState = Enemy_States.idle;
                return;
            }
            
        }
        
        //then continue

        // - update curr velocity
        curr_velocity = Math.Min(curr_velocity + Time.deltaTime * acceleration, max_velocity);
        // - update pos
        UpdatePosition(currFishTarget.transform.position, curr_velocity);

        //is our target within our attack_range
        float distance = Vector3.Distance(currFishTarget.transform.position, transform.position);
        if(distance < attack_range){

            //bite
            Bite();
        }
        
    }


    //bite mechanic
    //assuming we have a fish target
    //take our head object and dash it towards the target fish
    //then have body move after the head, like catch up
    private void Bite(){

        Debug.Log(gameObject.ToString() + "Bite");
        //get this gameobject's stats script and deal damage
        currFishTarget.GetComponent<FishStats_ParentClass>().TakeDamage(attackPower);
        
        //make particle
        Instantiate(bite_particle, transform.position, Quaternion.identity);
    }



    //when player clicks on bass we run this
    public new void OnPointerClick(PointerEventData eventData) {

        //run original function
        base.OnPointerClick(eventData);

        //reset 
        curr_velocity = 0;
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
