using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Movement : MonoBehaviour
{

    //--------------------------------- used in the update position function ---------------------------------
    private float idle_velocity = 1;
    private float hungry_velocity = 2;
    private float current_Vel = 0; 

    // --------------------------------- targetting ---------------------------------
    private Vector3 idleTarget;
    private float targetRadius = 0.5f;
    private float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position

    
    private GameObject foodTarget;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void IdleMode(){
        //move around the tank
        //get a random point on the screen

        var distance = Vector3.Distance(idleTarget, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            updatePosition(idleTarget);
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

            //find food to followe 
            var closestDis = float.PositiveInfinity;
            var allFoods = Controller_Food.instance.GetAllFood();

            //for all food objs in scene, get the closest
            var tempTarget = allFoods[0];
            foreach (GameObject food in allFoods){

                var newDis = (transform.position - food.transform.position).sqrMagnitude;

                if(newDis < closestDis){

                    closestDis = newDis;
                    tempTarget = food;  
                }
            }
            //if this is our first food target found, set instant
            //if this new food we found is closer, set that as new target
            //else nothing
            if(foodTarget == null){
                foodTarget = tempTarget;
            }
            else if(foodTarget != tempTarget){
                foodTarget = tempTarget;
            }
            
            //once the fish or the trash can gets to the food, the food destroysSelf(), and foodtarget = null again
        }
        //now
        //follow food
        //head towards target 
        updatePosition(foodTarget.transform.position);


    }

    private void NewRandomIdleTarget_Tank(){

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

    private void updatePosition(Vector3 targetTypePosition){

        //update physical position towards the target
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetTypePosition,
            current_Vel * Time.deltaTime
        );

        //everything now is sprite visuals

    }

}
