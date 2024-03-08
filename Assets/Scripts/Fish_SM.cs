
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
    [SerializeField] float velocity = 2;
    [SerializeField] Transform sprite;


    private Vector2 idleTarget;
    [SerializeField] float getNewTargetRange = 3f;


    [SerializeField] float stomach;
    [SerializeField] const int maxStomach = 100;
    [SerializeField] float burnRate = 30;
    [SerializeField] int hungryRange = 50;


    [SerializeField] Controller_Food controller_Food;
    [SerializeField] Controller_Fish controller_Fish;
    private GameObject foodTarget;
    private bool targetingFood = false;



    //Getting new targets within tank    
    private float tank_xLower = 0;
    private float tank_xUpper = 0;
    private float tank_yLower = 0;
    private float tank_yUpper = 0;


    // Start is called before the first frame update
    void Start()
    {

        fishCurrentState = Fish_States.idle;

        NewRandomIdleTarget_Tank();

        stomach = maxStomach;

    }

    // Update is called once per frame
    void Update()
    {

        //burn stomach
        stomach -= burnRate * Time.deltaTime;

        //check if fish starved to death
        if(stomach <= 0){
            Died();
        }

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

        //are we hungry and food is on screen
        if(stomach < hungryRange && controller_Food.GetFoodLength() > 0){
            ChangeState(Fish_States.hungry);
            return;
        }

        var distance = Vector2.Distance(idleTarget, transform.position);

        if(Mathf.Abs(distance) > getNewTargetRange){
            
            updatePosition(idleTarget);
        }

        //get new point once fish reaches it
        else{
            NewRandomIdleTarget_Tank();

        }
    }

    private void HungryMode(){
        //look for food, until full

        //is there food on screen to target
        if(controller_Food.GetFoodLength() == 0){
            ChangeState(Fish_States.idle);
            return;
        }

        //if wer not targeting food
        //or our current target is null
        //          : target a food
        if(!targetingFood || foodTarget == null){

            //find food to followe
            var closestDis = float.PositiveInfinity;
            var allFoods = controller_Food.GetAllFood();

            foreach (GameObject food in allFoods){

                var newDis = (transform.position - food.transform.position).sqrMagnitude;

                if(newDis < closestDis){

                    closestDis = newDis;
                    foodTarget = food;  
                }
            }

            targetingFood = true;
        }
        

        //follow food
        //head towards target 
        updatePosition(foodTarget.transform.position);
        


    }

    private void SleepyMode(){
        //look for a spot to sleep in

    }


    public void SetTankDem(float xL, float xU, float yL, float yU){

        tank_xLower = xL;
        tank_xUpper = xU;
        tank_yLower = yL;
        tank_yUpper = yU;
    }


    private void NewRandomIdleTarget_Tank(){
        idleTarget = new Vector2(
                Random.Range(tank_xLower, tank_xUpper),
                Random.Range(tank_yLower, tank_yUpper)
            );
    }


    private void updatePosition(Vector2 targetTypePosition){

        //head towards target 
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetTypePosition,
            velocity * Time.deltaTime
        );

        //sprite fliping
        if(transform.position.x - idleTarget.x < 0){
            sprite.localScale = new Vector3(-1f, 1f, 1f);
        }
        else{
            sprite.localScale = new Vector3(1f, 1f, 1f);
        }
    }


    private void OnTriggerStay2D(Collider2D other){

        if(Fish_States.hungry == fishCurrentState && other.gameObject.tag == "Food"){

            //eat
            controller_Food.TrashThisFood(other.gameObject);
            stomach = maxStomach;
            
        }
    }

    


    public void SetFoodController(Controller_Food food_c){
        controller_Food = food_c;
    }

    public void SetFishController(Controller_Fish fish_c){
        controller_Fish = fish_c;
    }

    public void Died(){

        controller_Fish.RemoveFish(gameObject);

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected() {
    
        //current target for fish
        Gizmos.color = new Color(1,1,0,0.75f);
        Gizmos.DrawWireSphere(idleTarget, getNewTargetRange);


        
    }

}
