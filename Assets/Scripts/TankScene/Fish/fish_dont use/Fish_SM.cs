using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

//hungry
//
//idle
//
//grabbed

//idle, walk around
//hungry, look out for food
//grabbed, let the player drag you around
public enum Fish_States {idle, hungry, grabbed, dropped};

public class Fish_SM : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    


    [SerializeField] Fish_States fishCurrentState;
    [SerializeField] List<Transform> sprite_transparency; //fish sprites
    [SerializeField] Transform fishObj_transform;   //whole fish object 
    [SerializeField] AudioClip dieSoundClip;
    [SerializeField] AudioClip sacrifice_success;
    [SerializeField] AudioClip sacrifice_fail;



    //--------------------------------- used in the update position function ---------------------------------
    private float idle_velocity = 1;
    private float hungry_velocity = 2;
    private float current_Vel = 0; 


    // --------------------------------- sprite movement ---------------------------------
    private float h_turningSpeed = 1.5f;
    private float v_turningSpeed = 1;
    private float startTime = 0;
    private float z_angle = 0; //previous z angle we had (should start at 0 angle)
    float z_angle_pivotTo = 0; //current z we are pivoting to
    private float zDepth = 0; //the z transform our fish swims at, to reference at flippings
    float y_angle = 0;

    // --------------------------------- targetting ---------------------------------
    private Vector3 idleTarget;
    private float targetRadius = 0.5f;
    private float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position


    // --------------------------------- hunger related --------------------------------- 
    private float stomach;
    private const int startStomach = 20;//total seconds before fish dies of hunger
    private float burnRate = 1; //per second (could be changed for other level types "fever")
    private int hungryRange = startStomach/2; 
    private float nextCheckCounter = 0; //seconds untilNextCheck for food target
    private GameObject foodTarget;


    // --------------------------------- audio related---------------------------------
    private bool play_Fail_SoundAgain = true;







    // Start is called before the first frame update
    void Start()
    {

        ChangeState(Fish_States.idle);

        NewRandomIdleTarget_Tank();

        stomach = startStomach;

        current_Vel = idle_velocity;

        zDepth = Controller_Food.instance.targetZ.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //if the fish is grabbed, this fish is immune to hunger
        if(Fish_States.grabbed != fishCurrentState){
            
            //burn stomach
            stomach -= burnRate * Time.deltaTime;

                //check if fish starved to death
            if(stomach <= 0){
                Died();
            }
            //if hungry
            else if(stomach < hungryRange && fishCurrentState != Fish_States.hungry){

                //change sprite transparancy
                ChangeTransparency(false);

                //if food on screen aswell, change to hungry state
                if(Controller_Food.instance.GetFoodLength() > 0){
                    //are we hungry and food is on screen
                    ChangeState(Fish_States.hungry);
                }

                //also check if this was a tutorial push
                Controller_Tutorial.instance.TutorialClick(ExpectType.Fish_Hungry);
                
            }
        }

        

        
        //enter state logic

        switch(fishCurrentState){
            case Fish_States.idle:
                IdleMode();
                break;
            case Fish_States.hungry:
                HungryMode();
                break;
            case Fish_States.grabbed:
                //do nothing?
                break;
            case Fish_States.dropped:
                //do nothing
                break;
            default:
                Debug.Log("No current state for fish");
                break;
        }
    }

    private void ChangeTransparency(bool setFullAlpha){

        //we have to check if its a skinned messrender, or a simple meshrender
        foreach(var sprite in sprite_transparency){

            try{
                //first we try simple messrender
                if(setFullAlpha){sprite.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,1,1,1));}
                else{sprite.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,1,1,0.5f));}
            }
            catch(MissingComponentException ){
                //else we use skinned mesh
                if(setFullAlpha){sprite.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Color(1,1,1,1));}
                else{sprite.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Color(1,1,1,0.5f));}
            } 
        }

        
    }


    public void ChangeState (Fish_States newState){

        fishCurrentState = newState;
        NewTargetVariables();

        if(newState == Fish_States.idle){
            current_Vel = idle_velocity;
        }
        else if(newState == Fish_States.hungry){
            current_Vel = hungry_velocity;
        }
        else if(newState == Fish_States.grabbed){
            current_Vel = 0;
        }
    }

    private void IdleMode(){
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

    private void HungryMode(){
        //look for food, until full
        //is there food on screen to target
        //if not then we can't chase food, return to idle state
        if(Controller_Food.instance.GetFoodLength() == 0){
            ChangeState(Fish_States.idle);
            return;
        }

        //if wer not targeting food (ie:current target food is null)
        //          : target a food
        if(foodTarget == null || 1 < nextCheckCounter){

            nextCheckCounter = 0;

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
                NewTargetVariables();
            }
            else if(foodTarget != tempTarget){
                foodTarget = tempTarget;
                NewTargetVariables();
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



    //-----------------------------MOUSE DRAGGING------------------------------------------------------------


    public void OnBeginDrag(PointerEventData data){
        //change to grabbed state
        ChangeState(Fish_States.grabbed);
        //reset verticle pos
        fishObj_transform.localRotation = Quaternion.Euler(0, y_angle, 0); 
        //allow for sound
        play_Fail_SoundAgain = true;
    }
    public void OnDrag(PointerEventData data){
        //now just follow the mouse position
        transform.position = Controller_Player.instance.mousePos;

    }
    public void OnEndDrag(PointerEventData data){
        //return to idle state
        ChangeState(Fish_States.dropped);
        //timer to switch to idel again
        IEnumerator co = IdleReturn();
        StartCoroutine(co);
    }
    private IEnumerator IdleReturn() {

        yield return new WaitForSeconds(0.1f);
        ChangeState(Fish_States.idle);
    }


    //-----------------------------------------------------------------------------------------







    private void NewRandomIdleTarget_Tank(){
        
        //update variables
        NewTargetVariables();

        //new target
        var curr_pos = new Vector3 (transform.position.x, transform.position.y, zDepth);

        //tanke dememsions
        var swimDem = TankCollision.instance.GetTankSwimArea();

        while(Mathf.Abs(Vector2.Distance(idleTarget, curr_pos)) < newTargetMinLengthRadius){
            
            idleTarget = new Vector3(
                Random.Range(swimDem.Item1, swimDem.Item2),
                Random.Range(swimDem.Item3, swimDem.Item4),
                zDepth
            );
        }
        
    }

    //whenever a new target is set
    //run this
    private void NewTargetVariables(){
        z_angle = z_angle_pivotTo; //set our current z angle as our new old z angle we start at
        startTime = Time.time;      //reset our turning time for lerp
    }


    private void updatePosition(Vector3 targetTypePosition){

        //update physical position towards the target
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetTypePosition,
            current_Vel * Time.deltaTime
        );

        

        //everything now is sprite visuals
        //in other words, updating the rotations on the fish, making it look like its actually swimming towards the target position//
        float z_curr_angle = (Time.time - startTime) / v_turningSpeed;
        float y_curr_angle = (Time.time - startTime) / h_turningSpeed;
        
        

        //fish local facing position (towards target) 
        //sprite (left or right)
        if(transform.position.x - targetTypePosition.x < 0){


            //turn right  (0 degrees to 180 degress)
            //if we are in the most left most position
            if(fishObj_transform.localRotation.eulerAngles.y == 0){
                y_angle = Mathf.SmoothStep(0, 180, y_curr_angle);
            }
            else{
            //else start at current y
                y_angle = Mathf.SmoothStep(fishObj_transform.localRotation.eulerAngles.y, 180, y_curr_angle);
            }
            
        }
        else if (transform.position.x - targetTypePosition.x > 0){

            //return to left (180 degress to 0 degrees)
            //if we are currently at the right most position
            if(fishObj_transform.localRotation.eulerAngles.y == 180){
                y_angle = Mathf.SmoothStep(180, 0 , y_curr_angle);
            }
            //else start at current y
            else{
                y_angle = Mathf.SmoothStep(fishObj_transform.localRotation.eulerAngles.y, 0 , y_curr_angle);
            }

        }
        else {
            //else keep curr pos rotation
            y_angle = fishObj_transform.localRotation.eulerAngles.y;
            //this shouldnt happen
            //so
            Debug.Log("Fish y_angle is not working");
        }
        
        //vertical rotation 
        //z angle is the previous angle we were using, its updated everytime we get a new target
        var maxAngle = 30;
        z_angle_pivotTo = Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - targetTypePosition.y, transform.position.x - targetTypePosition.x);//get the angle to pivot to
        z_angle_pivotTo = ClampAngle(z_angle_pivotTo, maxAngle); //clamp that to be a max of -30 to +30 degrees 
        z_angle_pivotTo = Mathf.Lerp(z_angle, z_angle_pivotTo, z_curr_angle);//smooth the angle (current z -> new pivot) get a value inbetween
        z_angle_pivotTo = ClampAngle(z_angle_pivotTo, maxAngle); //clamp that value again to -30 __ +30 max degrees


        //apply rotations
        fishObj_transform.localRotation = Quaternion.Euler(0, y_angle, z_angle_pivotTo); 
    }
    

    //for the current postion of the fish (depending on which way) get a new angle to pivot too, 
    //given a max angle pivot
    private float ClampAngle(float angle, float maxAngle){


        //negative check (up)
        if(angle < -maxAngle){
            return -maxAngle;
        }
        
        //positive check (down)
        if(angle > maxAngle){
            return maxAngle;
        }

        //else return the angle, since its between
        return angle;
    }

    


    private void OnTriggerStay2D(Collider2D other){

        //              FOOD
        //if fish is hungry and we collided with food
        if(Fish_States.hungry == fishCurrentState && other.gameObject.CompareTag("Food"))
        {

            //eat + destroy obj
            stomach = other.GetComponent<FoodValue>().GetFoodValue();
            Controller_Food.instance.TrashThisFood(other.gameObject);

            //did fish get full (enough)
            if(stomach > hungryRange){

                //return color to fish
                ChangeTransparency(true);

                //set our state to idle again
                ChangeState(Fish_States.idle);

                //eating ages fish
                gameObject.GetComponent<Fish_Age>().Ate();

                //check if this feeding was for fish to push tutorial
                Controller_Tutorial.instance.TutorialClick(ExpectType.Fish_Feed);
            }
        }

        //          DRAG - COMBINE
        //if the fish is being dropped, and we collide with another fish, and fish ages are same
        if( Fish_States.dropped == fishCurrentState &&
            other.gameObject.CompareTag("Fish") &&
            other.gameObject.GetComponent<Fish_Age>().current_age_stage == GetComponent<Fish_Age>().current_age_stage &&
            GetComponent<Fish_Age>().current_age_stage < Controller_Fish.instance.GetFishStages().Count-1
        ){

            //then combine the two
            //reset variables (health, hunger, ..)?
            //increase age by one
            GetComponent<Fish_Age>().Fish_Birthday();
            
            //also kill the other fish
            other.gameObject.GetComponent<Fish_SM>().Died(false);

        }


        
        

    }

   


    public void Died(bool playSound = true){

        //removes self from the list of current fish known to the fish controller
        Controller_Fish.instance.RemoveFish(gameObject);
        
        //play die sound
        if(playSound){AudioManager.instance.PlaySoundFXClip(dieSoundClip, transform, 1f);}
        

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected() {
    
        //current target for fish
        Gizmos.color = new Color(1,1,0,0.75f);
        Gizmos.DrawWireSphere(idleTarget, targetRadius);

        //current target for fish
        Gizmos.color = new Color(0,1,1,0.75f);
        Gizmos.DrawWireSphere(transform.position, newTargetMinLengthRadius);


        
    }


    private bool dontUseThisFunction(){
        return play_Fail_SoundAgain;
    }


}
