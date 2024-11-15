using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private const int attackPower = 1;  //out of 100 health from fish, fish dies in 3 hits

    // ----------------------------------------------- movement -----------------------------------------------
    private float curr_r_vel = 0; // current rotational velocity of starfish
    private const float r_accel = 1; //rotational acceleration per second
    private float curr_sprite_r = 0; //current sprite z axis rotation

    private bool spinning = false; // do we have enough wind up built up to attack target fish
    private const float vel_threshold = 4; //velocity threshold needed for starfish to burst "_units per s"
    public float burst_vel = 3;    //burst velocity starfish moves at towards target



    private new void Start() {
        base.Start();
        
        //set target fish
        currFishTarget = Controller_Fish.instance.GetRandomFish();

    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();

        
        //if we have a fish to attack, start wind up
        //and should we continue wind up
        if(currFishTarget != null && !spinning){
            Debug.Log("building up rot");

            //build wind up, and update sprite rotation
            curr_r_vel += r_accel * Time.deltaTime;
            curr_sprite_r = curr_sprite_r + curr_r_vel;
            sprite_transform.localRotation = Quaternion.Euler(0,0, curr_sprite_r);

            //did we reach rotational velocity threshold 
            //set our attack bool to true, to not build anymore wind up
            if(curr_r_vel > vel_threshold)
                {
                    Debug.Log("attack_");
                    spinning = true;
                    
                    //spin move
                    var target_dir = (currFishTarget.position - transform.position).normalized;
                    rb.AddForce(target_dir * burst_vel, ForceMode2D.Impulse);
                }
            
        }
        else{
            //else, is there a fish to target?
            currFishTarget = Controller_Fish.instance.GetRandomFish();
            return;
        }

    }

    private void ResetAttack(){

        spinning = false;
        curr_r_vel = 0;

    }


    private new void OnTriggerEnter2D(Collider2D other) {


        //boundry collision
        //if we hit the tank edge so we lose our speed, so we restart rampping
        if(other.gameObject.CompareTag("Boundry")){

            ResetAttack();
            
        }


        //if we are not currently doing our spin move, then return
        //did we collide with fish
        if(spinning && other.gameObject.CompareTag("Guppy")){

            //animation
            Instantiate(bite_particle, transform.position, Quaternion.identity);
            //sound?

            //attack this fish
            other.gameObject.GetComponent<Guppy_Stats>().TakeDamage(attackPower);

            ResetAttack();

        }


        
        
    }


}
