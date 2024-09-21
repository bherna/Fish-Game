using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Movement : MonoBehaviour
{

    //--------------------------------- used in the update position function ---------------------------------
    private float idle_velocity = 1;
    private float hungry_velocity = 2;

    // --------------------------------- targetting ---------------------------------
    private Vector3 idleTarget;
    private float targetRadius = 0.5f;
    private float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position
    private GameObject foodTarget;


    // --------------------------------- Sprite ---------------------------------
    [SerializeField] Transform guppy_transform;   //get transform of guppy sprite
    private float startTime;
    private float h_turningSpeed = 1.5f;
    float y_angle = 0;


    // Start is called before the first frame update
    void Start()
    {
        NewRandomIdleTarget_Tank();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //move around the tank
    //get a random point on the screen
    public void IdleMode(){

        var distance = Vector3.Distance(idleTarget, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            updatePosition(idleTarget, idle_velocity);
        }

        //get new point once fish reaches it
        else{
            NewRandomIdleTarget_Tank();

        }
    }

    public void HungryMode(){

        //if wer not targeting food (ie:current target food is null) 
        //          : target a food
        if(foodTarget == null){
            NewFoodTarget_Tank();
        }

        //if food target is still null
        if(foodTarget == null){
            //run idel mode
            IdleMode();
        }
        else{
            //else
            //follow food
            //head towards target 
            updatePosition(foodTarget.transform.position, hungry_velocity);
        }


    }

    private void NewRandomIdleTarget_Tank(){

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

    private void NewFoodTarget_Tank(){

        //new target
        NewTargetVariables();

        //find food to followe 
        var closestDis = float.PositiveInfinity;
        var allFoods = Controller_Food.instance.GetAllFood();
        if(allFoods.Count == 0){return;}

        //for all food objs in scene, get the closest
        var tempTarget = allFoods[0];
        foreach (GameObject food in allFoods){

            var newDis = (transform.position - food.transform.position).sqrMagnitude;

            if(newDis < closestDis){

                closestDis = newDis;
                tempTarget = food;  
            }
        }
        //
        foodTarget = tempTarget;
        
        //once the fish or the trash can gets to the food, the food destroysSelf(), and foodtarget = null again
    }

    private void updatePosition(Vector3 targetTypePosition, float current_Vel){

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
            y_angle = Mathf.SmoothStep(guppy_transform.localRotation.eulerAngles.y, 180, y_curr_angle);
            
        }
        else if (transform.position.x - targetTypePosition.x > 0){

            //return to left (180 degress to 0 degrees)
            y_angle = Mathf.SmoothStep(guppy_transform.localRotation.eulerAngles.y, 0 , y_curr_angle);

        }
        else {
            //else keep curr pos rotation
            y_angle = guppy_transform.localRotation.eulerAngles.y;
            //this shouldnt happen
            //so
            Debug.Log("Guppy y_angle is not working");
        }


        //apply rotations
        guppy_transform.localRotation = Quaternion.Euler(0, y_angle, 0); 


    }



    //whenever a new target is set we reset our sprite variables
    private void NewTargetVariables(){
        startTime = Time.time;      //reset our turning time for lerp
    }

    private void OnDrawGizmosSelected() {
    
        //current target for fish
        Gizmos.color = new Color(1,1,0,0.75f);
        Gizmos.DrawWireSphere(idleTarget, targetRadius);

        //current target for fish
        Gizmos.color = new Color(0,1,1,0.75f);
        Gizmos.DrawWireSphere(transform.position, newTargetMinLengthRadius);


        
    }
}
