using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/*

    Desc: Every school of fish has to have at least one teacher.
    Ability: When ever a new enemy wave starts, they call all fish over to them away from enemies
    Rarity: quest

*/
/// </summary>

public class Pet_SchoolTeacher : Pet_ParentClass
{

    /// ----------------------------- protect mode -------------------------------///
    private float curr_whistle_timer = 0; // time keep of current seconds till next guppy call
    private float whistle_cooldn = 3f; // how many seconds long til the next whistle call
    [SerializeField] AudioClip whistle_audio;
    private float protect_velocity = 1.8f;
    //private Vector3 target_position_farthest;


    
    private Event_Type event_type = Event_Type.enemyWave;
    

    //School Teacher pet
    // ability :
    //          When ever a new enemy wave starts, 
    //          The school teacher will call all guppies in tank to itself
    //          and keep them huddled untill the enemies are all gone 
    //          but won't be able to keep hungry guppies from straying

    //          The school teacher will try to avoid the enemies as much as
    //          possible. 


    private new void Start() {
        base.Start(); //still start init variables from parent class


    }

    //
    private new void Update() {
        base.Update();


        //idle when not currently in enemy wave
        //else enter protect mode
        switch(curr_PetState){

            case Pet_States.idle:
                IdleMode();
                break;
            case Pet_States.protect:
                ProtectMode();
                break;

            default:
                Debug.Log(gameObject.ToString()+" pet is not in a current state");
                break;
        }

    }


    //when the enemy wave starts, this pet will enter protect mode
    private void Enter_ProtectMode(){
 
        //enter protect mode
        curr_PetState = Pet_States.protect;

    }
    private void ProtectMode(){

        //update position
        TargetAwayFromEnemies();

        //call guppies whistle logic
        curr_whistle_timer += Time.deltaTime;

        if(curr_whistle_timer >= whistle_cooldn){
            curr_whistle_timer = 0; // reset timer
            CallGuppies();
        }

        //other stuff

    }
    //when the last enemy is killed, run this
    private void Exit_ProtectMode(){

        //return to idle mode
        curr_PetState = Pet_States.idle;

        //reset curr whistle timer
        curr_whistle_timer = 0;

        //let guppies disperse
        Controller_Fish.instance.SchoolTeacherWhistle_Disperse();
    }


    //new way to find next update position
    //pet school teacher wants to move in a circluar path
    // ------------------------------------------------RIGHT NOW IM NOT DOING THAT, JUST DO IDEL MOVEMENT SINCE TO MUCH WORK
    private void TargetAwayFromEnemies(){

        var distance = Vector3.Distance(idleTarget, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            updatePosition(idleTarget, protect_velocity);
        }

        //get new point once fish reaches it
        else{
            NewRandomIdleTarget_Tank();

        }

    }


    //other half of the school teacher ability
    //call all guppies to this pet
    //all this should do, is make guppies change target type to this pet transform
    //      every few seconds the school teacher will sound its whisle to call any stray guppies to it
    private void CallGuppies(){

        //guppy whistle sound
        AudioManager.instance.PlaySoundFXClip(whistle_audio, transform, 1f);
        //guppy function call
        Controller_Fish.instance.SchoolTeacherWhistle_Huddle(gameObject);
    }

    //when ever enemy waves start, we enter protect mode
    public override void Event_Init(Event_Type type, GameObject obj){ 

        if(type != event_type){return;}
        Enter_ProtectMode();
    }
    //when ever enemy waves are over, we exit protect mode
    public override void Event_EndIt(Event_Type type){

        if(type != event_type){return;}
        //exit protect mode
        Exit_ProtectMode();
    }

    
}
