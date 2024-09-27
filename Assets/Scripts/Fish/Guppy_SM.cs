using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//hungry
//
//idle
//
//grabbed

//idle, walk around
//hungry, look out for food
//grabbed, let the player drag you around
public enum Guppy_States {idle, hungry, follow};

public class Guppy_SM : MonoBehaviour
{

    // --------------------------------- gubby script reference --------------------------------- 
    private Guppy_Movement guppy_Movement;


    // --------------------------------- other ---------------------------------
    //current state
    /// <summary>
    /// Idle overrides Hungry state : if no food is available on screen (state stays as hungry)
    
    /// </summary>
    public Guppy_States guppy_current_state {
        get;   // get method
        private set ; // set method
    }

    // Start is called before the first frame update
    void Start()
    {
        guppy_Movement = GetComponent<Guppy_Movement>();
    }

    // Update is called once per frame
    void Update()
    {

        // -------------------------------------------------------- movement related -------------------------------------------------------- // 
        //which state we should enter
        Guppy_States enterState = guppy_current_state;

        //if are current state is hungry and no fish, just go idle 
        if(enterState == Guppy_States.hungry && Controller_Food.instance.GetFoodLength() == 0){
            enterState = Guppy_States.idle;
        }

        //enter state logic
        switch(enterState){
            case Guppy_States.idle:
                guppy_Movement.IdleMode();
                break;
            case Guppy_States.hungry:
                guppy_Movement.HungryMode();
                break;
            case Guppy_States.follow:
                guppy_Movement.FollowMode();
                break;
            default:
                Debug.Log("No current state for guppy");
                break;
        }
    }

    public void GuppyToHungry(){
        guppy_current_state = Guppy_States.hungry;
    }
    public void GuppyToIdle(){
        guppy_current_state = Guppy_States.idle;
    }
    public void GuppyToFollow(){
        guppy_current_state = Guppy_States.follow;
    }

}
