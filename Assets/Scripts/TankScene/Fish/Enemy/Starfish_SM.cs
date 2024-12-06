using UnityEngine.EventSystems;
using UnityEngine;


public class Starfish_SM : Enemy_ParentClass, IPointerClickHandler
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

    // ----------------------------------------------- attack -----------------------------------------------

    private const int attackPower = 2;  //number of bites it takes for starfish to eat guppy

    // ----------------------------------------------- movement -----------------------------------------------
    private float curr_r_vel = 0; // current rotational velocity of starfish
    private const float r_accel = 1; //rotational acceleration per second
    private float curr_sprite_r = 0; //current sprite z axis rotation

    private bool spinning = false; // do we have enough wind up built up to attack target fish
    private const float vel_threshold = 4; //velocity threshold needed for starfish to burst "_units per s"
    private float burst_vel = 1;    //burst velocity starfish moves at towards target



    private float stunTimer = 0;



    private new void Start() {
        base.Start();
        
        //set target fish
        currFishTarget = Controller_Fish.instance.GetRandomFish();

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
            return;
        }


        //we technically only need to do attack mode when we are building up
        //rotational velocity, which we use to sling shot our selfs around
        //we become blind when we are charging (and return when we anything)
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
                rb.AddForce(target_dir * burst_vel, ForceMode2D.Impulse);
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
            ResetAttack();
        }
    }

    private void ResetAttack(){

        spinning = false;
        curr_r_vel = 0;

    }


    private void TakeDamage(int damage){

        curr_health -= damage;
        if(curr_health <= 0){
            Died();
        }
    }


    private new void OnTriggerEnter2D(Collider2D other) {


        //boundry collision
        //if we hit the tank edge so we lose our speed, so we restart rampping
        //we still use ontriggerSTAY2d for returning to center
        if(other.gameObject.CompareTag("Boundry")){

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

            ResetAttack();
            return;

        }


        //this happens when we entercounter counter
        //player tag should be with the controller_player obj
        if(curr_EnemyState == Enemy_States.stunned && other.gameObject.CompareTag("Player")){

            Debug.Log(string.Format("KB da starfish."));

            //kb the starfish
            Vector2 kbVector = (transform.position - other.gameObject.transform.position).normalized;

            rb.AddForce(kbVector * kbForce, ForceMode2D.Impulse);

            //return to idle
            curr_EnemyState = Enemy_States.idle;
            Controller_Player.instance.DeleteTrail();

            TakeDamage(Controller_Player.instance.Get_GunDamage() *2); //for now we'll just double the damage or what ever

            ResetAttack();
            return; //added for comformateee

        }
        
    }

    public new void OnPointerClick(PointerEventData eventData){


        //if the game is paused, return
        if(Controller_EscMenu.instance.paused){
            return;
        }

        //first check for posible counter attack move
        if(spinning){

            //change starfish state
            stunTimer = Controller_Player.trailDuration;
            curr_EnemyState = Enemy_States.stunned;

            //create flash of light, to show that we countered
            Debug.Log(string.Format("COUNTERED"));//debug for now iguess

            //we stop moving all together
            rb.velocity = Vector3.zero;

            //we create a trail particle
            //for the player
            Controller_Player.instance.CreateTrail();


        }
        else{

            //create gun particle
            Controller_Player.instance.Run_GunParticle();


            //knockback
            Vector2 kbVector = (transform.position - eventData.pointerCurrentRaycast.worldPosition).normalized;

            rb.AddForce(kbVector * kbForce, ForceMode2D.Impulse);

            //damage
            TakeDamage(Controller_Player.instance.Get_GunDamage());
        }


    }










}
