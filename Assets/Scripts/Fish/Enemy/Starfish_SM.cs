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

    private Transform target_position; //reference to target fish, we just care for position
    private int damageAmount = 35;  //out of 100 health from fish, fish dies in 3 hits

    private float curr_r_vel = 0; // current rotational velocity of starfish
    private float r_accel = 1; //rotational acceleration per second
    private float curr_sprite_r = 0; //current sprite z axis rotation

    private bool spinning = false; // do we have enough wind up built up to attack target fish
    private float vel_threshold = 4; //velocity threshold needed for starfish to burst "_units per s"
    private float phy_LinearDrag = 1.2f;


    private float burst_vel = 40;    //burst velocity starfish moves at towards target
    private List<GameObject> fishes_attacked;
    private float bounce_vel = 0.2f;
    private (float, float, float, float) boundry_d;


    private new void Start() {
        base.Start();
        
        //set target fish
        target_position = Controller_Fish.instance.GetRandomFish();

        //set fish attacked list to empty
        fishes_attacked = new List<GameObject>();

        //set our boundry collider size from tank
        boundry_d = TankCollision.instance.GetBoundryArea();

        //set drag for player clicks (f)
        rb.drag = phy_LinearDrag;
    }

    // Update is called once per frame
    public void Update()
    {
        //if we have a fish to attack, start wind up
        //and should we continue wind up
        if(target_position != null && !spinning){

            //build wind up, and update sprite rotation
            curr_r_vel += r_accel * Time.deltaTime;
            curr_sprite_r = curr_sprite_r + curr_r_vel;
            sprite_transform.localRotation = Quaternion.Euler(0,0, curr_sprite_r);

            //did we reach rotational velocity threshold 
            //set our attack bool to true, to not build anymore wind up
            if(curr_r_vel > vel_threshold)
                {
                    spinning = true;
                    
                    //remove all drag for starfish
                    rb.drag = 0;
                    
                    //spin move
                    var target_dir = (target_position.position - transform.position).normalized;
                    rb.AddForce(target_dir * burst_vel, ForceMode2D.Force);
                }
            
        }
        else{
            //else, is there a fish to target?
            target_position = Controller_Fish.instance.GetRandomFish();
            return;
        }


    //

    }



    private void OnTriggerStay2D(Collider2D other) {

        //if we are not currently doing our spin move, then return
        if(!spinning){return;}
        
        //did we collide with fish
        if(other.gameObject.CompareTag("Fish")){

            //is this a new fish our starfish 'collided' with
            if(! Is_In_List(other.gameObject)){

                //animation
                Instantiate(bite_particle, transform.position, Quaternion.identity);
                //attack this fish
                other.gameObject.GetComponent<Guppy_Stats>().TakeDamage(damageAmount);
                //and add to list
                fishes_attacked.Add(other.gameObject);

            }           
            

        }
    }


    private void OnTriggerEnter2D(Collider2D other) {

        //it doesnot matter if the star fish is currently in spin move or not, just bouce ;

        //if we hit the tank edge
        if(other.gameObject.CompareTag("Boundry")){

            //set our velocity towards middle of tank
            var x = rb.velocity.x;
            var y = rb.velocity.y;

            if( transform.position.x + rb.velocity.x < boundry_d.Item1 + sprite_transform.localScale.x ||
                transform.position.x + rb.velocity.x > boundry_d.Item2 - sprite_transform.localScale.x)
                {
                    x = -x;
                }
            if( transform.position.y + rb.velocity.y < boundry_d.Item3 + sprite_transform.localScale.y*2 ||
                transform.position.y + rb.velocity.y > boundry_d.Item4 - sprite_transform.localScale.y*2  )
                {
                    y = -y;
                } 

            //apply new bounce velocity
            Vector2 newVel = new Vector2(x * bounce_vel, y *bounce_vel);
            rb.velocity = newVel;


            //only reset vars if we did spin move
            if(spinning){
                //reset spin build variables
                curr_r_vel = 0;
                curr_sprite_r = 0;
                sprite_transform.localRotation = Quaternion.Euler(0,0,0);

                //reset fish list, so we can hurt fish again :(
                fishes_attacked = new List<GameObject>();

                //add drag aswell so the starfish slows down
                rb.drag = phy_LinearDrag;

                //now, we are not spinnig move
                spinning = false;
            }

            
        }
    }


    //returns true if a fish gameobject given is in our fishes attacked list
    private bool Is_In_List(GameObject fish){

        for(int i = 0; i < fishes_attacked.Count; i++){

            if(GameObject.ReferenceEquals(fish, fishes_attacked[i])){
                return true;
            }
        } 
        return false;
    }

}
