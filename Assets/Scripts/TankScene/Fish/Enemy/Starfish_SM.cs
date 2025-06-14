using UnityEngine.EventSystems;
using UnityEngine;
using Steamworks;


public class Starfish_SM : Enemy_ParentClass 
{

    //star fish
    //star fish attack by first winding up there velocity (spin up)
    //once they gain enough 'rotational' velocity, they turn that into kinetic energy
    //by throwing themselves at their target.

    //
    //1) set target
    //2) gain rotation vel      \ repeat these two until target dies, then start from begining.
    //3) burst towards target   /


    [SerializeField] ParticleSystem bite_particle;
    [SerializeField] Starfish_Collider starfish_coll;

    // ----------------------------------------------- attack -----------------------------------------------

    private const int attackPower = 2;  //number of bites it takes for starfish to eat guppy

    // ----------------------------------------------- movement -----------------------------------------------
    private float curr_r_vel = 0; // current rotational velocity of starfish
    private const float r_accel = 1; //rotational acceleration per second
    private float curr_sprite_r = 0; //current sprite z axis rotation

    private bool spinning = false; // do we have enough wind up built up to attack target fish
    private const float vel_threshold = 4; //velocity threshold needed for starfish to burst "_units per s"
    private float burst_vel = 8f;    //burst velocity starfish moves at towards target

    private float linearDrag = 0; //this is set at start, dont edit the value

    private float stunTimer = 0;


    private new void Start() {
        base.Start();
        
        //set target fish
        currFishTarget = Controller_Fish.instance.GetRandomFish();

        linearDrag = rb.drag;

    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();


        switch(curr_EnemyState){

            case Enemy_States.idle:

                //in idle, we are going to make the starfish look like its sticking to the tank glass
                //animation 
                Idle();
                break;
            
            case Enemy_States.attack:
                Attak();
                break;

            case Enemy_States.stunned:
                Stunned();
                break;

            default:
                Debug.Log("Starfish has no state");
                break;
        }

        
        

    }

    //in idle we check if new fish are available to attack
    //else just stick to the wall
    private void Idle(){

        currFishTarget = Controller_Fish.instance.GetRandomFish();

        if(currFishTarget != null){
            curr_EnemyState = Enemy_States.attack;
        }
        else{

            //wall stickin time
            
        }
    }

    private void Attak(){

        //first check our fish target
        if(currFishTarget == null){
            //go to idle
            curr_EnemyState = Enemy_States.idle;
            starfish_coll.SetOrientation(Enemy_States.idle, 0);
            ResetAttack(); //else we can cheat winding up 
            return;
        }


        //we technically only need to do attack mode when we are building up
        //rotational velocity, which we use to sling shot our selfs around
        //we become blind when we are charging (and return when we hit anything)
        if(!spinning){ 

            //build wind up, and update sprite rotation
            curr_r_vel += r_accel * Time.deltaTime;
            curr_sprite_r = curr_sprite_r + curr_r_vel;
            sprite_transform.localRotation = Quaternion.Euler(0,0, curr_sprite_r);

            //did we reach rotational velocity threshold 
            //set our attack bool to true, to not build anymore wind up
            if(curr_r_vel > vel_threshold)
            {
                spinning = true;
                
                //spin move
                var target_dir = (currFishTarget.position - transform.position).normalized;
                rb.velocity = target_dir * burst_vel;
                rb.drag = 0; //we remove drag, just so we dont get stuck in the middle of the tank (since we expect some obstacle to reset our attack)

                //allso set our collider orientation here finally
                int angle = (int)(Mathf.Atan2(currFishTarget.position.y - transform.position.y, currFishTarget.position.x - transform.position.x) * Mathf.Rad2Deg);
                starfish_coll.SetOrientation(Enemy_States.attack, angle);
            }
            
        }


    }


    private void Stunned(){

        //we just count down in here, (the initial stunned reason should set the timer)
        //if we reach 0, then we return to idle, idle should take care of the rest

        //also while stunned 
        //we are open to counter attack from player light trail

        stunTimer -= Time.deltaTime;

        if(stunTimer <= 0){
            //done
            curr_EnemyState = Enemy_States.idle;
            starfish_coll.SetOrientation(Enemy_States.idle, 0);
            ResetAttack();
        }
    }

    private void ResetAttack(){

        spinning = false;
        curr_r_vel = 0;
        rb.drag = linearDrag; //reset drag, since we dont want to be gliding everywhere
    }



    public override void Coll_OnTrigger(Collider2D other) {


        //boundry collision
        //if we hit the tank edge so we lose our speed, so we restart rampping
        //we still use ontriggerSTAY2d for returning to center
        if(other.gameObject.CompareTag("Boundry")){

            starfish_coll.SetOrientation(Enemy_States.idle, 0);
            ResetAttack();
            return;
        }

        //if we are not currently doing our spin move, then return
        //did we collide with fish
        if(spinning && (other.gameObject.CompareTag("Guppy") || other.gameObject.CompareTag("Tutorial"))){

            //animation
            Instantiate(bite_particle, transform.position, Quaternion.identity);
            //sound?

            //attack this fish
            other.gameObject.GetComponent<Guppy_Stats>().TakeDamage(attackPower);

            starfish_coll.SetOrientation(Enemy_States.idle, 0);
            ResetAttack();
            return;

        }


        //this happens when we entercounter counter (trail);p
        //ie, this is after the player already clicked on the starfish, causing them to get stunned, 
        // now we are expecting for player counter trail to hit us
        if(curr_EnemyState == Enemy_States.stunned && other.gameObject.CompareTag("Player")){

            //kb the starfish (using the player distance traveld with trail)
            Vector2 kbVector = (transform.position - other.gameObject.transform.position).normalized;
            rb.AddForce(kbVector * Controller_Player.instance.GetDistanceTraveled_Value(), ForceMode2D.Impulse);

            //update animation
            Debug.Log(string.Format("DIstance: {0}", Controller_Player.instance.GetDistanceTraveled_Value()));

            //return to idle
            curr_EnemyState = Enemy_States.idle;
            starfish_coll.SetOrientation(Enemy_States.idle, 0);
            Controller_Player.instance.DeleteTrail();

            TakeDamage(Controller_Player.instance.Get_GunDamage() *2); //for now we'll just double the damage or what ever

            ResetAttack();
            return; //added for comformateee

        }
        
    }

    public override void OnPointerClick(PointerEventData eventData){


        //if the game is paused, return
        if(Controller_EscMenu.instance.paused){
            return;
        }

        //first check for posible counter attack move
        if(spinning){

            //change starfish state
            stunTimer = Controller_Player.trailDuration;
            curr_EnemyState = Enemy_States.stunned;
            starfish_coll.SetOrientation(Enemy_States.stunned, 0);

            //create flash of light, to show that we countered

            //we start to move backwards super slowly
            Vector2 kbVector = (transform.position - eventData.pointerCurrentRaycast.worldPosition).normalized;
            rb.velocity = kbVector * kbForce_stunned;

            //init a ripple particle
            Controller_Ripple.instance.CreateRipple(Controller_Player.instance.transform.position);

            //we create a trail particle
            //for the player
            Controller_Player.instance.CreateTrail();

            //WE DONT RESET ATTACK HERE
            //since that get done when the starfish is counter attacked or exits stun state
        }
        else{
            //else we just do basic attack (from parent class)
            base.OnPointerClick(eventData);

            //also reset attack here, else starfish can instantly attack next guppy
            ResetAttack();
        }


    }










}
