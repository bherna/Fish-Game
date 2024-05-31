
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
    [SerializeField] Transform sprite_transparency; //reference to fish sprite
    [SerializeField] Transform fishObj_transform;   //reference to fish object


    //used in the update position function
    [SerializeField] float idle_velocity = 1;
    [SerializeField] float hungry_velocity = 2;
    private float current_Vel = 0; 

    [SerializeField] float h_turningSpeed = 1;
    [SerializeField] float v_turningSpeed = 1;
    private float startTime = 0;


    private Vector2 idleTarget;
    [SerializeField] float getNewTargetRange = 3f;


    [SerializeField] float stomach;
    [SerializeField] const int startStomach = 70;
    [SerializeField] float burnRate = 30;
    [SerializeField] int hungryRange = 50;
    private float nextCheckCounter = 0; //seconds untilNextCheck for food target


    [SerializeField] Controller_Food controller_Food;
    [SerializeField] Controller_Fish controller_Fish;
    private GameObject foodTarget;



    //Getting new targets within tank    
    private float tank_xLower = 0;
    private float tank_xUpper = 0;
    private float tank_yLower = 0;
    private float tank_yUpper = 0;


    // Start is called before the first frame update
    void Start()
    {

        ChangeState(Fish_States.idle);

        NewRandomIdleTarget_Tank();

        stomach = startStomach;

        current_Vel = idle_velocity;

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
        //if hungry
        else if(stomach < hungryRange && fishCurrentState != Fish_States.hungry){

            //change sprite transparancy
            //sprite.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,1,1,0.5f));
            sprite_transparency.gameObject.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Color(1,1,1,0.5f));

            //if food on screen aswell, change to hungry state
            if(controller_Food.GetFoodLength() > 0){
                //are we hungry and food is on screen
                ChangeState(Fish_States.hungry);
            }
            
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
        startTime = Time.time;

        if(newState == Fish_States.idle){
            current_Vel = idle_velocity;
        }
        else if(newState == Fish_States.hungry){
            current_Vel = hungry_velocity;
        }
    }

    private void IdleMode(){
        //move around the tank
        //get a random point on the screen

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
        //if not then we can't chase food, return to idle state
        if(controller_Food.GetFoodLength() == 0){
            ChangeState(Fish_States.idle);
            return;
        }

        //if wer not targeting food (ie:current target food is null)
        //          : target a food
        if(foodTarget == null || 1 < nextCheckCounter){

            nextCheckCounter = 0;

            //find food to followe 
            var closestDis = float.PositiveInfinity;
            var allFoods = controller_Food.GetAllFood();

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


        //update next check counter, 
        nextCheckCounter += Time.deltaTime;


    }

    private void SleepyMode(){
        //look for a spot to sleep in
        //not in use yet

    }


    public void SetTankSwimDimensions(float xL, float xU, float yL, float yU){

        tank_xLower = xL;
        tank_xUpper = xU;
        tank_yLower = yL;
        tank_yUpper = yU;
    }


    private void NewRandomIdleTarget_Tank(){
        
        //update variables
        NewTargetVariables();

        //new target
        idleTarget = new Vector2(
                Random.Range(tank_xLower, tank_xUpper),
                Random.Range(tank_yLower, tank_yUpper)
            );
    }

    //whenever a new target is set
    //run this
    private void NewTargetVariables(){
        startTime = Time.time;
    }


    private void updatePosition(Vector2 targetTypePosition){

        //head towards target 
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetTypePosition,
            current_Vel * Time.deltaTime
        );

        
        float z_curr_angle = (Time.time - startTime) / v_turningSpeed;
        float y_curr_angle = (Time.time - startTime) / h_turningSpeed;
        float y_angle;
        float z_angle;

        //fish local facing position (towards target) 
        //sprite (left or right)
        //aswell verticality
        if(transform.position.x - targetTypePosition.x < 0 && fishObj_transform.localRotation.eulerAngles.y < 180){
            //turn right
            y_angle = Mathf.SmoothStep(0, 180, y_curr_angle);
  
        }
        else if (transform.position.x - targetTypePosition.x > 0){
            //return to left
            y_angle = Mathf.SmoothStep(fishObj_transform.localRotation.eulerAngles.y, 0 , y_curr_angle);

        }
        else {
            //else keep curr pos rotation
            y_angle = fishObj_transform.localRotation.eulerAngles.y;
        }
        
        //vertical rotation 
        z_angle = Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - targetTypePosition.y, transform.position.x - targetTypePosition.x);
        z_angle = ClampAngle(z_angle, 30);
        z_angle = Mathf.SmoothStep(0, z_angle, z_curr_angle);
        fishObj_transform.localRotation = Quaternion.Euler(0, y_angle, z_angle);
    }

    //given a max angle, check if our angle goes outside that bound (checks both - and +)
    private float ClampAngle(float angle, float maxAngle){

        //negative check
        if(angle < -maxAngle){
            return -maxAngle;
        }

        if(angle > maxAngle){
            return maxAngle;
        }

        //else return the angle, since its between
        return angle;
    }

    private int LR_Angle(float angle){

        if(angle < 90){
            return 0;
        }
        else{
            return 180;
        }
    }


    private void OnTriggerStay2D(Collider2D other){

        if(Fish_States.hungry == fishCurrentState && other.gameObject.CompareTag("Food"))
        {

            //eat + destroy obj
            stomach += other.GetComponent<FoodValue>().GetFoodValue();
            controller_Food.TrashThisFood(other.gameObject);

            //did fish get full (enough)
            if(stomach > hungryRange){

                //return color to fish
                sprite_transparency.gameObject.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Color(1,1,1,1));

                //set our state to idle again
                ChangeState(Fish_States.idle);

                gameObject.GetComponent<Fish_Age>().Ate();
            }
            

            
        }
    }

    


    //functions used in other scripts
    public void SetFoodController(Controller_Food food_c){
        controller_Food = food_c;
    }

    public void SetFishController(Controller_Fish fish_c){
        controller_Fish = fish_c;
    }

    public void Died(){

        //removes self from the list of current fish known to the fish controller
        controller_Fish.RemoveFish(gameObject);

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected() {
    
        //current target for fish
        Gizmos.color = new Color(1,1,0,0.75f);
        Gizmos.DrawWireSphere(idleTarget, getNewTargetRange);


        
    }


    public Controller_Fish GetControllerFish(){
        return controller_Fish;
    }

}
