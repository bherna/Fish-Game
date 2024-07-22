using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Starfish_SM : MonoBehaviour, IPointerClickHandler
{

    //star fish
    //star fish attack by first winding up there velocity (spin up)
    //once they gain enough 'rotational' velocity, they turn that into kinetic energy
    //by throwing themselves at their target.

    //
    //1) set target
    //2) gain rotation vel      \ repeat these two until target dies, then start from begining.
    //3) burst towards target   /

    [SerializeField] Transform sprite;
    [SerializeField] Rigidbody2D rb;

    private Transform target_position; //reference to target fish, we just care for position
    private int damageAmount = 35;  //out of 100 health from fish, fish dies in 3 hits

    private int health = 8; // in terms of clicks

    private float curr_r_vel = 0; // current rotational velocity of starfish
    private float r_accel = 1; //rotational acceleration per second
    private float curr_sprite_r = 0; //current sprite z axis rotation

    private bool spinning = false; // do we have enough wind up built up to attack target fish
    private float vel_threshold = 4; //velocity threshold needed for starfish to burst "_units per s"


    private float burst_vel = 40;    //burst velocity starfish moves at towards target
    private List<GameObject> fishes_attacked;
    private float bounce_vel = 0.4f;
    private (float, float) boundry_d;


    private void Start() {
        
        //set target fish
        target_position = Controller_Fish.instance.GetRandomFish();

        //set fish attacked list to empty
        fishes_attacked = new List<GameObject>();

        //set our boundry collider size from tank
        boundry_d = TankCollision.instance.GetBoundryArea();
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
            sprite.localRotation = Quaternion.Euler(0,0, curr_sprite_r);

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


        //**not attached to previous if, as an else-if, since we want to return on no target
        if(spinning){

            //while spinnig our movement velocity will drop down to zero, once we touch a tank edge


            //once we are done doing spin move, reset fish attacked list

        }

    }



    public void OnPointerClick(PointerEventData eventData){


        //if the game is paused, return
        if(Controller_Main.instance.paused){
            return;
        }

        //damage
        health -= Controller_Player.instance.Get_GunDamage();

        //die
        if(health <= 0){
            Died();
        }
        
    }

    //fish died
    //will be more than one way to die
    private void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //die
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {

        //if we are not currently doing our spin move, then return
        if(!spinning){return;}
        
        //did we collide with fish
        if(other.gameObject.CompareTag("Fish")){

            //is this a new fish our starfish 'collided' with
            if(! Is_In_List(other.gameObject)){
                //attack this fish
                other.gameObject.GetComponent<Fish_Stats>().TakeDamage(damageAmount);
                //and add to list
                fishes_attacked.Add(other.gameObject);

            }           
            

        }
    }


    private void OnTriggerExit2D(Collider2D other) {

        //it doesnot matter if the star fish is currently in spin move or not, just bouce ;

        Debug.Log("boundry hit");

        //if we hit the tank edge
        if(other.gameObject.CompareTag("Boundry")){

            //set our velocity towards middle of tank
            var x = rb.velocity.x;
            var y = rb.velocity.y;

            if( boundry_d.Item1/2 + rb.velocity.x > boundry_d.Item1 - sprite.localScale.x || 
                boundry_d.Item1/2 + rb.velocity.x < sprite.localScale.x)
                {
                    x = -x;
                }
            if( boundry_d.Item2 - 2 + rb.velocity.y > boundry_d.Item2 - sprite.localScale.y ||
                boundry_d.Item2 - 2 + rb.velocity.y < sprite.localScale.y)
                {
                    y = -y;
                } 

            //apply new bounce velocity
            Vector2 newVel = new Vector2(x * bounce_vel, y *bounce_vel);
            rb.velocity = newVel;

            //reset spin build variables
            curr_r_vel = 0;
            curr_sprite_r = 0;
            sprite.localRotation = Quaternion.Euler(0,0,0);

            //reset fish list, so we can hurt fish again :(
            fishes_attacked = new List<GameObject>();

            //add drag aswell so the starfish slows down
            rb.drag = 0.5f;

            //now, we are not spinnig move
            spinning = false;
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
