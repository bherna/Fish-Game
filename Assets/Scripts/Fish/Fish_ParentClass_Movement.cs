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
    [SerializeField] protected BoxCollider2D attach_pos;    //fill this if we have an attachment, example: this will be the mouth transform
                                                            //make sure the transform position of the attachment is 0,0,0 
    protected float idle_velocity = 1;
    
    
    protected void updatePosition(Vector3 targetTypePosition, float current_Vel){

        try{
            //try with a given transform_pos, 
            //update physical position towards the target
            transform.position = Vector2.MoveTowards( transform.position, new Vector2(targetTypePosition.x - attach_pos.offset.x, targetTypePosition.y - attach_pos.offset.y)  , current_Vel * Time.deltaTime );

        }catch(UnassignedReferenceException){

            //else do the same but without target_pos
            //update physical position towards the target
            transform.position = Vector2.MoveTowards( transform.position, targetTypePosition, current_Vel * Time.deltaTime );
        }
        

        //----------------- everything now is sprite visuals ------------------------------
        float y_curr_angle = (Time.time - startTime) / h_turningSpeed;

        //fish local facing position (towards target) 
        //sprite (left or right)
        if(transform.position.x - targetTypePosition.x < 0){

            //turn right  (0 degrees to 180 degress)
            y_angle = Mathf.SmoothStep(sprite_transform.localRotation.eulerAngles.y, 180, y_curr_angle);
            
        }
        else if (transform.position.x - targetTypePosition.x > 0){

            //return to left (180 degress to 0 degrees)
            y_angle = Mathf.SmoothStep(sprite_transform.localRotation.eulerAngles.y, 0 , y_curr_angle);

        }
        else {
            //else keep curr pos rotation
            y_angle = sprite_transform.localRotation.eulerAngles.y;
            //this shouldnt happen
            //so
            Debug.Log(gameObject.ToString() + " y_angle is not working");
        }


        //apply rotations
        sprite_transform.localRotation = Quaternion.Euler(0, y_angle, 0); 


    }

    protected virtual void Update() {

        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if(Controller_Main.instance.paused){
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
