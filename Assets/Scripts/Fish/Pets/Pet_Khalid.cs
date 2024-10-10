using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// 
//    Desc: An immortal jellyfish, Khalid is your never dieing best friend that you can always rely on once per enemy wave
//    Ability:  When ever a new enemy wave starts, Khalid will become one target that enemies can go after
//              If Khalid dies from an enemy, he goes into his polyp mode
//              In Polyp mode, he is untargetable and slowly regains his health, once healed he is back to being targetable
//    Rarity: Level Questing
//
/// </summary>
/// 



public class Pet_Khalid : Pet_ParentClass
{


    // --------------------- stats ------------------------- //
    private int curr_health = 0;
    private const int max_health = 20; 


    //

    private float curr_seconds = 0; //used in both protect mode and polyp mode, reset as needed
    private const float secondsInPolyp = 10;
    private const float secondsTillCall = 2;
    private Vector2 bot_of_tank = new Vector2(0, 4.5f);
    private Event_Type event_type = Event_Type.enemyWave;
    private bool inPolyp = false;

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

        curr_health = max_health;
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();

        switch(curr_PetState){

            case Pet_States.idle:
                IdleMode();
                break;
            case Pet_States.protect:
                ProtectMode();
                break;
            case Pet_States.ability:
                PolypMode();
                break;
            
            default:
                Debug.Log(gameObject.ToString() + " currently not in state.");
                break;
        }
    }


    public void TakeDamage(int damage){
        curr_health -= damage;

        if(curr_health <= 0){
            Debug.Log("Khalid died");
            curr_PetState = Pet_States.ability;

            //reset our seconds, 
            curr_seconds = 0;

            //change enemy target away from us
            Controller_Enemy.instance.GetEnemyAtIndex(0).GetComponent<Enemy_ParentClass>().SetTargetFish(Controller_Fish.instance.GetRandomFish());

        }
    }

    //in protect mode, periodically send a message to the first enemy in the controller_enemy list
    //make them target us
    //
    private void ProtectMode(){

        //send message
        curr_seconds += Time.deltaTime;
        if(curr_seconds >= secondsTillCall){
            Debug.Log("called..");
            Controller_Enemy.instance.GetEnemyAtIndex(0).GetComponent<Enemy_ParentClass>().SetTargetFish(transform); //call
            curr_seconds = 0; //reset
        }

        //movement
        //after that, we just move normally around the tank, i guess
        IdleMode();

    }


    //enter hibernating  mode essentially
    //we are dead pretty much and need to regain health
    private void PolypMode(){

        // --------------------------- heading to bottom of tank code -------------------------- //
        var distance = Vector3.Distance(bot_of_tank, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            updatePosition(idleTarget, idle_velocity);
            return;
        }
        //animation related
        //we we didn't start the polyp anitaion, we done it once here
        else if(!inPolyp){
            inPolyp = true;
            //udate animtaion

        }
        //else we are close enough
        //start polyp mode
        // ------------------------------------- in polyp ----------------------------------- //
        
        //update timer
        curr_seconds += Time.deltaTime;
        

        if(curr_seconds >= secondsInPolyp){

            //we are reborn
            curr_health = max_health;
            curr_PetState = Pet_States.idle; //bug ?,    rightnow we can only die once per wave, since we can't re-enter protect mode from middle of wave

            //update animation

            //reset curr seconds
            curr_seconds = 0;
        }

    }


    //when ever a new wave starts, khalid becomes targetable for enemies
    //ender protect mode
    public override void Event_Init(Event_Type type, GameObject obj){

        //if we didn't get our event type, return
        //if current pet state is set to ability (ie: we are hibernating ), return
        if(type != event_type ||
            curr_PetState == Pet_States.ability
        ){return;}
        
        //
        curr_PetState = Pet_States.protect;
        curr_seconds = 1.5f; //call right away

        //animation updated
    }
    //wave ends, return to idle
    public override void Event_EndIt(Event_Type type){
        //if we didnt get our event type, return
        //if we are NOT currently in protect mode, return
        if(type != event_type || 
            curr_PetState != Pet_States.protect){return;}

        curr_PetState = Pet_States.idle;
        
        //animation updated
    }
}
