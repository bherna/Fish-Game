using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Pet_States {idle, protect, grabbed, dropped};

public class Pet_ParentClass : MonoBehaviour
{

    //-----------------------------                 -----------------------------------------//
    protected Pet_States curr_PetState;


    //--------------------------------- used in the update position function ---------------------------------
    protected float idle_velocity = 1;

    // --------------------------------- targetting ---------------------------------
    protected Vector3 idleTarget;
    protected float targetRadius = 0.5f;
    protected float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position


    // --------------------------------- Sprite ---------------------------------
    [SerializeField] protected Transform pet_transform;   //get transform of pet sprite
    protected float startTime;
    protected float h_turningSpeed = 1.5f;
    protected float y_angle = 0;



    // Start is called before the first frame update
    protected void Start()
    {
        NewRandomIdleTarget_Tank();
        startTime = Time.time;
        curr_PetState = Pet_States.idle;
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    //move around the tank
    //get a random point on the screen
    protected void IdleMode(){

        var distance = Vector3.Distance(idleTarget, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            updatePosition(idleTarget, idle_velocity);
        }

        //get new point once fish reaches it
        else{
            NewRandomIdleTarget_Tank();

        }
    }


    protected void NewRandomIdleTarget_Tank(){

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
            y_angle = Mathf.SmoothStep(pet_transform.localRotation.eulerAngles.y, 180, y_curr_angle);
            
        }
        else if (transform.position.x - targetTypePosition.x > 0){

            //return to left (180 degress to 0 degrees)
            y_angle = Mathf.SmoothStep(pet_transform.localRotation.eulerAngles.y, 0 , y_curr_angle);

        }
        else {
            //else keep curr pos rotation
            y_angle = pet_transform.localRotation.eulerAngles.y;
            //this shouldnt happen
            //so
            Debug.Log("Pet y_angle is not working");
        }


        //apply rotations
        pet_transform.localRotation = Quaternion.Euler(0, y_angle, 0); 


    }



    //whenever a new target is set we reset our sprite variables
    protected void NewTargetVariables(){
        startTime = Time.time;      //reset our turning time for lerp
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
