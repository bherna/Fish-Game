using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_SchoolTeacher : Pet_ParentClass
{

    /// ----------------------------- protect mode -------------------------------///
    private float curr_whistle_timer = 0; // time keep of current seconds till next guppy call
    private float whistle_cooldn = 3f; // how many seconds long til the next whistle call
    [SerializeField] AudioClip whistle_audio;
    private float protect_velocity = 1.3f;
    private Vector3 target_position_farthest;
    
    
    

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
                Debug.Log("School teacher pet is not in a current state");
                break;
        }

    }


    //when the enemy wave starts, this pet will enter protect mode
    private void Enter_ProtectMode(){
        Debug.Log("protect");
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
    //pet school teacher wants to move away from enemies
    private void TargetAwayFromEnemies(){

        //get new target far away from enemies
        

        //update position
        updatePosition(target_position_farthest, protect_velocity);

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
    public override void Event_EnemyWaveStart(){ 
        Debug.Log("child overide event");
        Enter_ProtectMode();
    }
    //when ever enemy waves are over, we exit protect mode
    public override void Event_EnemyWaveEnd(){

        //exit protect mode
        Exit_ProtectMode();
    }

    
}
