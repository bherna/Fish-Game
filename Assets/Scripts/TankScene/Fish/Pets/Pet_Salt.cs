using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// 
//    Desc: A helper feeder fish, Salt takes on the task of occasionally feeding fish in the tank
//    Ability:  After _ amount of seconds, Salt has a food charge
//              When ever a guppy gets hungry, Salt will use their food charge to spawn a peice of food for them
//              Salt pellet has the same stats as a level 2 food
//    Rarity: Level Questing
//
/// </summary>
/// 


public class Pet_Salt : Pet_ParentClass
{


    [SerializeField] GameObject salt_ref;

    private const float throwStr = 40;
    private bool charged = false; //if salt is ready to feed guppy
    private bool saltInTank = false; // we start with no salt in tank
    private float sec_tillCharged = 0;
    private const float totalSecsTillCharged = 8; 

    private List<GameObject> guppyList; //holds a list of all currenlty hungry guppys
    private bool facingTarget = false; //true when salt is facing guppy that is currenlty hunger
    private Vector3 faceingVec;





    private new void Start(){

        base.Start();

        guppyList = new List<GameObject>();
        faceingVec = Vector3.zero;
    }




    // Update is called once per frame
    private new void Update()
    {
        base.Update();

        //build up food charge
        //onlyyy if we dont have salt in tank already
        if(!charged && !saltInTank){
            sec_tillCharged += Time.deltaTime;
            if(sec_tillCharged >= totalSecsTillCharged ){
                charged = true;
            }
        }
    

        switch(curr_PetState){

            case Pet_States.idle:
                IdleMode();
                break;
            case Pet_States.ability:
                AbilityMode();
                break;
            default:
                Debug.Log(gameObject.ToString()+" has no state to enter");
                break;
        }
    }



    //if we are abilty mode
    //we currently have pet to feed
    //we just stop moving
    //face towards the guppy that called hungry
    //and throw a salt pellet at them
    //return to idle
    private void AbilityMode(){

        //if we have NO charge
        // OR we have salt_pellet already in tank
        if( !charged || saltInTank ){
            //we
            IdleMode();
        }
        //make sure we have atleast one guppy that's hungry
        else if(guppyList.Count > 0){

            //check incase our guppy died and isnt removed from list
            //we could remove all null guppys at once here, but there is an issue if i try doing that
            if(guppyList[0] == null){
                guppyList.RemoveAt(0);
                return;
            }


            //start facing guppy
            if(!facingTarget){

                //first get a position that faces guppy
                if(faceingVec == Vector3.zero){
                    GetGuppyDirection();
                }
                
                //start turning LERP(slowly)
                if(UpdatePosition(faceingVec, idle_velocity)){
                    //once facing
                    facingTarget = true;
                }

            }
            else{
                //we good//
                //throw food
                ThrowFood();

                //now there is food in tank
                saltInTank = true;

                //remove from list
                //reset values
                guppyList.RemoveAt(0);      //guppy
                facingTarget = false;       //sprite
                faceingVec = Vector3.zero;  //sprite
                sec_tillCharged = 0;        //food
            }
            
        }
        else{
            //return to idle
            curr_PetState = Pet_States.idle;
            //get new idle target, else this pet looks like its stuck going back and forth
            NewRandomIdleTarget_Tank();
        }

    }

    private void ThrowFood(){

        //throw food
        //first we create the instance
        //then we add to food controller
        var food = Instantiate(salt_ref, transform.position, quaternion.identity);
        Controller_Food.instance.AddFood_Gameobject(food, false);

        //get direction
        //throw
        Vector2 dir = (guppyList[0].transform.position - transform.position).normalized;
        food.GetComponent<Rigidbody2D>().AddForce(dir * throwStr);

        //remove charge
        charged = false;
    }

    private void GetGuppyDirection(){

        faceingVec = Vector2.MoveTowards(transform.position, guppyList[0].transform.position, 0.5f);
    }

    //-------------- abstract functions -----------------------------//
    //functions to call all pets in tank
    //whether or not they need to do something with this annoucement
    public override void Event_Init(Event_Type type, GameObject obj){

        if(type == Event_Type.guppyHungry){
            //change to feeding mode
            curr_PetState = Pet_States.ability;
            guppyList.Add(obj);
        }

        if(type == Event_Type.saltDestroyed){
            //let salt spawn a new salt_food pellet
            saltInTank = false;
        }
    }
    public override void Event_EndIt(Event_Type type){
        //nothing, since we just return to idle after feeding
    }
}
