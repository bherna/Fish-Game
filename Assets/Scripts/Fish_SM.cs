
using Unity.VisualScripting;
using UnityEngine;

public class Fish_SM : MonoBehaviour
{

    //hungry
    //
    //idle
    //
    //sleepy

    //idle, walk around
    public enum Fish_States {idle, hungry, sleepy};

    [SerializeField] Fish_States fishCurrentState;
    [SerializeField] Collision2D fishCollision;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float velocity = 2;
    [SerializeField] Transform sprite;


    private Vector2 target;
    [SerializeField] float getNewTargetRange = 3f;


    [SerializeField] float stomach;
    [SerializeField] const int maxStomach = 100;
    [SerializeField] float burnRate = 2;
    [SerializeField] int hungryRange = 50;


    // Start is called before the first frame update
    void Start()
    {
        fishCurrentState = Fish_States.idle;

        NewTarget_Tank();

        stomach = maxStomach;

    }

    // Update is called once per frame
    void Update()
    {
        switch(fishCurrentState){
            case Fish_States.idle:
                IdleMode();
                break;
            case Fish_States.hungry:
                HungryMode();
                break;
            case Fish_States.sleepy:
                SleepyMode();
                break;
            default:
                Debug.Log("No current state for fish");
                break;
        }
    }


    public void ChangeState (Fish_States newState){

        fishCurrentState = newState;
    }

    private void IdleMode(){
        //move around the tank
        //get a random point on the screen

        //are we hungry
        if(stomach < hungryRange){
            ChangeState(Fish_States.hungry);
            return;
        }

        var distance = Vector2.Distance(target, transform.position);

        if(Mathf.Abs(distance) > getNewTargetRange){
            
            //head towards target 
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                velocity * Time.deltaTime
            );

            //sprite fliping
            if(transform.position.x - target.x < 0){
                sprite.localScale = new Vector3(-1f, 1f, 1f);
            }
            else{
                sprite.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        //get new point once fish reaches it
        else{
            NewTarget_Tank();

            //and lower stomach
            stomach -= burnRate;
        }
    }

    private void HungryMode(){
        //look for food, until full

        

    }

    private void SleepyMode(){
        //look for a spot to sleep in

    }

    private void NewTarget_Tank(){
        target = new Vector2(
                Random.Range(-8.9f, 9.0f),
                Random.Range(-4.5f, 4.5f)
            );
    }
}
