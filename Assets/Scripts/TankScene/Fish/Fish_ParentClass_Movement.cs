using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_ParentClass_Movement : MonoBehaviour
{

    // --------------------------------- Sprite ---------------------------------
    [SerializeField] protected Transform sprite_transform;   //get transform of pet sprite
    protected float startTime;
    protected float h_turningSpeed = 1.5f;
    protected float y_angle = 0;


    // --------------------------------- Targeting ---------------------------------
    protected Vector3 idleTarget;
    protected float targetRadius = 0.5f;
    protected float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position
    protected float idle_velocity = 1;
    
    
    //returns true when position is achieved
    protected bool updatePosition(Vector3 target_pos, float current_Vel, bool use3=false){

        //vector 3 vs vector2
        if(use3){
            //update physical position towards the target
            transform.position = Vector3.MoveTowards( transform.position, target_pos, current_Vel * Time.deltaTime );
        }
        else{
            //update physical position towards the target
            transform.position = Vector2.MoveTowards( transform.position, target_pos, current_Vel * Time.deltaTime );
        }
        

        //----------------- everything now is sprite visuals ------------------------------
        float y_curr_angle = (Time.time - startTime) / h_turningSpeed;

        //fish local facing position (towards target) 
        //sprite (left or right)
        if(transform.position.x - target_pos.x < 0){

            //turn right  (0 degrees to 180 degress)
            y_angle = Mathf.SmoothStep(sprite_transform.localRotation.eulerAngles.y, 180, y_curr_angle);
            
        }
        else if (transform.position.x - target_pos.x > 0){

            //return to left (180 degress to 0 degrees)
            y_angle = Mathf.SmoothStep(sprite_transform.localRotation.eulerAngles.y, 0 , y_curr_angle);

        }
        else {
            //else keep curr pos rotation
            //y_angle = sprite_transform.localRotation.eulerAngles.y;
            //if we have no reason to turn
            //then we are in final position
            return true;
        }


        //apply rotations
        sprite_transform.localRotation = Quaternion.Euler(0, y_angle, 0); 

        return false;
    }

    protected virtual void Update() {

        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if(Controller_EscMenu.instance.paused){
            return;
        }
    }



    protected virtual void NewRandomIdleTarget_Tank(){

        //since new target
        NewTargetVariables();

        //new target
        var curr_pos = new Vector3 (transform.position.x, transform.position.y, 0);

        //tanke dememsions
        var swimDem = TankCollision.instance.GetTankSwimArea();

        while(Mathf.Abs(Vector2.Distance(idleTarget, curr_pos)) < newTargetMinLengthRadius){
            
            idleTarget = new Vector3(
                Random.Range(swimDem.Item1, swimDem.Item2),
                Random.Range(swimDem.Item3, swimDem.Item4), 
                0
            );
        }
        
    }

    //whenever a new target is set we reset our sprite variables
    protected virtual void NewTargetVariables(){
        startTime = Time.time;      //reset our turning time for lerp
    }

    protected void OnDrawGizmosSelected() {
    
        //if idle is null, dont show it
        if(idleTarget == null || idleTarget == new Vector3(0,0,0)){
            //dont show
        }
        else{
            //current target for fish
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(idleTarget, targetRadius);
        }
        

        //current range untill new target
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, newTargetMinLengthRadius);


        
    }
}
