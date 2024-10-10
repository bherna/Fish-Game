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
    
    
    protected void updatePosition(Vector3 targetTypePosition, float current_Vel){

        //update physical position towards the target
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetTypePosition,
            current_Vel * Time.deltaTime
        );

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



    protected void OnDrawGizmosSelected() {
    
        //current target for fish
        Gizmos.color = new Color(1,1,0,0.75f);
        Gizmos.DrawWireSphere(idleTarget, targetRadius);

        //current target for fish
        Gizmos.color = new Color(0,1,1,0.75f);
        Gizmos.DrawWireSphere(transform.position, newTargetMinLengthRadius);


        
    }
}
