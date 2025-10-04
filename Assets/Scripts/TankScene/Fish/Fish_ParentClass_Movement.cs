using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_ParentClass_Movement : MonoBehaviour
{

    // --------------------------------- Sprite ---------------------------------
    [SerializeField] protected Transform sprite_transform;   //get transform of pet sprite
    protected float startTime;
    protected float h_turningSpeed = 1.5f;
    protected float y_angle = 0;


    // --------------------------------- Targeting ---------------------------------
    protected Vector3 idleTarget;
    protected float targetRadius = 0.5f;
    protected float newTargetMinLengthRadius = 6; //the minimum length away from our fish current position
    protected float idle_velocity = 1;


    //references
    protected Rigidbody2D rb;

    //used in update position function for determining if we are going to use addforce or just update litteral position
    protected bool IStatic = true;
    //same as above but for part 2 of code: profile vs non profiled viewd fish code
    protected bool IProfile = true;


    
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }


    protected virtual void Update() {

        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if(Controller_EscMenu.instance.paused){
            return;
        }
    }




    /// <summary>
    /// This is our profile update position method, if we have a fish that doesn't need its head to be turned in the 
    /// direction it's heading towards, use the next method updateposition_z
    /// 
    /// This method returns true once its head is orienting in its final state, not truly nessasary for most logic 
    /// </summary>
    /// <param name="target_pos">where our fish is heading towards </param>
    /// <param name="current_Vel">since our fishes will use different velocities within their code, we dont want to hard set it</param>
    /// <param name="IStatic">whether our fish is going to need static or dynamic movement, some fish will just need to get to a position while others can be interactive within tank</param>
    /// <param name="IProfile">if we are a profile oriented fish or not, used in determining if we should run part 2 logic</param>
    /// <param name="useZ">if we are using vector3 or vector2 </param>
    /// <returns></returns>
    protected bool UpdatePosition(Vector3 target_pos, float current_Vel, bool useZ = false)
    {
        
        //---------------- step 1 of update position is updating the position: ------------------
        //vector 3 vs vector2
        if (useZ)
        {
            //if we are a static or dynamic fish
            if (IStatic)
            {
                transform.position = Vector3.MoveTowards(transform.position, target_pos, current_Vel * Time.deltaTime);
            }
            else
            {
                var dir = (target_pos - transform.position).normalized;
                rb.AddForce(dir * current_Vel * Time.deltaTime, ForceMode2D.Force);
            }
        }
        else
        {

            if (IStatic)
            {
                transform.position = Vector2.MoveTowards(transform.position, target_pos, current_Vel * Time.deltaTime);
            }
            else
            {
                var dir = (target_pos - transform.position).normalized;
                rb.AddForce(dir * current_Vel * Time.deltaTime, ForceMode2D.Force);
            }
        }




        //if we are NOT FALSE, or true, we contineou
        if (!IProfile) { return true; }

        //----------------- everything now is sprite visuals ------------------------------
        float y_curr_angle = (Time.time - startTime) / h_turningSpeed;

        //fish local facing position (towards target) 
        //sprite (left or right)
        if (transform.position.x - target_pos.x < 0)
        {

            //turn right  (0 degrees to 180 degress)
            y_angle = Mathf.SmoothStep(sprite_transform.localRotation.eulerAngles.y, 180, y_curr_angle);

        }
        else if (transform.position.x - target_pos.x > 0)
        {

            //return to left (180 degress to 0 degrees)
            y_angle = Mathf.SmoothStep(sprite_transform.localRotation.eulerAngles.y, 0, y_curr_angle);

        }
        else
        {
            //else keep curr pos rotation
            //y_angle = sprite_transform.localRotation.eulerAngles.y;
            //if we have no reason to turn
            //then we are in final position
            return true;
        }


        //apply rotations
        sprite_transform.localRotation = Quaternion.Euler(0, y_angle, 0);

        return false;
    }



    //create a new idle target, that is within the tank dimensions and outside the fish range.
    protected virtual void NewRandomIdleTarget_Tank()
    {

        //since new target
        NewTargetVariables();

        //new target
        var curr_pos = new Vector3(transform.position.x, transform.position.y, 0);

        //tanke dememsions
        var swimDem = TankCollision.instance.GetTankSwimArea();

        while (Mathf.Abs(Vector2.Distance(idleTarget, curr_pos)) < newTargetMinLengthRadius)
        {

            idleTarget = new Vector3(
                Random.Range(swimDem.Item1, swimDem.Item2),
                Random.Range(swimDem.Item3, swimDem.Item4),
                0
            );
        }

    }

    //whenever a new target is set we reset our sprite variables
    protected virtual void NewTargetVariables(){
        startTime = Time.time;      //reset our turning time for lerp
    }

    protected void OnDrawGizmosSelected() {
    
        //if idle is null, dont show it
        if(idleTarget == null || idleTarget == new Vector3(0,0,0)){
            //dont show
        }
        else{
            //current target for fish
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(idleTarget, targetRadius);
        }
        

        //current range untill new target
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, newTargetMinLengthRadius);


        
    }
}
