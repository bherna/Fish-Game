using UnityEngine;


//Each pet will be in one of these states at all times. 
public enum Pet_States {idle, protect, grabbed, dropped, ability, stun, depressed, stage1, stage2};
//Whenever something in the tank need to talk to the pets to trigger a state we use one of the following
public enum Event_Type {enemyWave, coin, guppyHungry, saltDestroyed, pearlCollected, guppyDead}

public abstract class Pet_ParentClass : Fish_ParentClass_Movement
{

    //-----------------------------                 -----------------------------------------//
    public Pet_States curr_PetState { protected set; get; }

    



    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        NewRandomIdleTarget_Tank();
        curr_PetState = Pet_States.idle;
    }

    //move around the tank
    //get a random point on the screen
    protected void IdleMode(){

        var distance = Vector3.Distance(idleTarget, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            UpdatePosition(idleTarget, idle_velocity);
        }

        //get new point once fish reaches it
        else{
            NewRandomIdleTarget_Tank();

        }
    }



    //idk how to get the tank trash can to talk to cherry
    public virtual void OnTouchGround()
    {

    }



    //-------------- abstract functions -----------------------------//
    //functions to call all pets in tank
    //whether or not they need to do something with this annoucement
    public abstract void Event_Init(Event_Type type, GameObject obj); //do nothing
    public abstract void Event_EndIt(Event_Type type); //do nothing

    
}
