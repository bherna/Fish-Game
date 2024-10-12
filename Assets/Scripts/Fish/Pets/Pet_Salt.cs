using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// 
//    Desc: A helper feeder fish, Salt takes on the task of occasionally feeding fish in the tank
//    Ability:  After _ amount of seconds, Salt has a food charge
//              When ever a guppy gets hungry, Salt will use one of their charges to instance a peice of food 
//              Salt pellet has the same stats as a level 2 food
//    Rarity: Level Questing
//
/// </summary>
/// 


public class Pet_Salt : Pet_ParentClass
{


    [SerializeField] GameObject salt_ref;
    private float throwStr = 70;
    private int salt_chargeCount = 0;
    private float sec_tillCharge = 0;
    private const float sec_forCharge = 4; 
    private Event_Type event_type = Event_Type.food;
    private List<GameObject> guppyList; //incase if multiple guppies call hunger
    private bool facingTarget = false;
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

        //if we have a charge, dont bother with this
        if(salt_chargeCount <= 0){
                sec_tillCharge += Time.deltaTime;
            if(sec_tillCharge >= sec_forCharge ){
                salt_chargeCount += 1;
                sec_tillCharge = 0;
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

        //also make sure we have a charge to eat
        if(salt_chargeCount <= 0){
            //if no charge, then run idle
            IdleMode();
        }
        else if(guppyList.Count > 0){

            //check incase our guppy died and isnt removed from list
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
                
                //start turning LERP
                if(updatePosition(faceingVec, idle_velocity)){
                    //once facing
                    facingTarget = true;
                }

            }
            else{
                //we good//
                //throw food
                ThrowFood();

                //remove from list
                //and reset target
                facingTarget = false;
                faceingVec = Vector3.zero;
                guppyList.RemoveAt(0);
            }
            
        }
        else{
            //return to idle
            curr_PetState = Pet_States.idle;
            //get new idle target
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
        salt_chargeCount -= 1;
    }

    private void GetGuppyDirection(){

        faceingVec = Vector2.MoveTowards(transform.position, guppyList[0].transform.position, 0.5f);
    }

    //-------------- abstract functions -----------------------------//
    //functions to call all pets in tank
    //whether or not they need to do something with this annoucement
    public override void Event_Init(Event_Type type, GameObject obj){

        if(type == event_type){
            //change to feeding mode
            curr_PetState = Pet_States.ability;
            guppyList.Add(obj);
        }
    }
    public override void Event_EndIt(Event_Type type){
        //nothing, since we just return to idle after feeding
    }
}
