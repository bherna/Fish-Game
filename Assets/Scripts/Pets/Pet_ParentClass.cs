using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Pet_States {idle, protect, grabbed, dropped};

public abstract class Pet_ParentClass : Fish_ParentClass
{

    //-----------------------------                 -----------------------------------------//
    protected Pet_States curr_PetState;


    //--------------------------------- used in the update position function ---------------------------------
    protected float idle_velocity = 1;

    // --------------------------------- targetting ---------------------------------
    protected Vector3 idleTarget;
    protected float targetRadius = 0.5f;
    protected float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position


    



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


    //-------------- abstract functions -----------------------------//
    //functions to call all pets in tank
    //whether or not they need to do something with this annoucement
    public abstract void Event_EnemyWaveStart(); //do nothing
    public abstract void Event_EnemyWaveEnd(); //do nothing

    
}
