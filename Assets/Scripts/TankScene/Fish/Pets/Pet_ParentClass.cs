using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Pet_States {idle, protect, grabbed, dropped, ability};
public enum Event_Type {enemyWave, coin, food}

public abstract class Pet_ParentClass : Fish_ParentClass_Movement
{

    //-----------------------------                 -----------------------------------------//
    protected Pet_States curr_PetState;

    



    // Start is called before the first frame update
    protected void Start()
    {
        NewRandomIdleTarget_Tank();
        startTime = Time.time;
        curr_PetState = Pet_States.idle;
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


    



    //-------------- abstract functions -----------------------------//
    //functions to call all pets in tank
    //whether or not they need to do something with this annoucement
    public abstract void Event_Init(Event_Type type, GameObject obj); //do nothing
    public abstract void Event_EndIt(Event_Type type); //do nothing

    
}
