using System;
using System.Collections;
using Assests.Inputs;
using UnityEngine;


public class Controller_Player : MonoBehaviour
{

    [SerializeField] ParticleSystem Gun_particle;
    [SerializeField] GameObject trail;


    //since this is public, if any ui elemnt needs it position when clicked, we can use this
    //public Vector3 mousePos = new Vector3(0,0,0);
    //THIS  IS  OLD, we are now  using a virtual mouse
    //use:
    //CustomVirtualCursor.GetMousePosition_V2() or  V3 for vector3


    // ---------------------------- player stats ------------------------------------------
    //gun strength
    private int gunPower = 1;
    //how long it takes for our gun to reload
    private int recoilTime = 3;
    //number of bulllets total the gun can hold
    private const int MaxBullets = 5;
    //current bullets holding at given time
    private int currBullets = 5;

    //curret cursor speed debuff
    public float cursorSpeed_debuff = 0; 



    //counter / trail related
    private float distanceTraveled = 0;
    private bool isTrailActive = false;
    public const int trailDuration = 1; //in seconds how long we can counter for(this is used in enemy code aswell to keep in sync)
    private Vector2 LastPosition;
    private int trailTotal = 0; // this keeps track of current chained trails
    private Coroutine timer;


    


    //single ton this class
    public static Controller_Player instance { get; private set; }

    private void Awake()
    {

        //delete duplicate of this instance

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }






    private void Start()
    {

        
    }

    private void Update()
    {
        //will have the collider still follwing virtual mouse now
        transform.position = CustomVirtualCursor.GetMousePosition_V2();

        //if we have a trail active, we want to update our distance traveld
        if (isTrailActive)
        {
            //update distance counter
            Vector2 newPosition = CustomVirtualCursor.GetMousePosition_V2();
            distanceTraveled += (newPosition - LastPosition).magnitude;
            LastPosition = newPosition;
        }

    }


    //this is used to slow down player mouse from debuffs,
    //give a percentage value of how much the debuff slows the player mouse down
    public void GiveMouseSpdStatusEffect(float percentage)
    {
        cursorSpeed_debuff += percentage;

        //like Value = 100% - percentage;
        int newSpeed = CustomVirtualCursor.cursorSpeed_playerSet *(int)(1 - Math.Clamp(percentage, 0, 1));

        CustomVirtualCursor.cursorSpeed_current = Math.Clamp(newSpeed, 0, CustomVirtualCursor.cursorSpeed_playerSet);;
    }


    // get sets
    public void Upgrade_GunPower()
    {
        gunPower += 1;
    }

    public int Get_GunDamage()
    {
        return gunPower;
    }


    public void Run_GunParticle()
    {
        Instantiate(Gun_particle, CustomVirtualCursor.GetMousePosition_V2(), Quaternion.identity);
    }

    //--------------------------------






    //------------------------------ counter attack code here  -------------------------------


    //used in counters, create a trail of light that the player needs to return towards the enemy
    //the longer the loop they create, the harder the damage back at the enemy
    public void CreateTrail()
    {

        //we don't need multiple trails, pointless
        if (isTrailActive)
        {
            //we don't want to reset distance travel'd
            trailTotal += 1;

            StopCoroutine(timer);
            timer = StartCoroutine(trailCountdown());
        }
        else
        {
            isTrailActive = true;

            //make sure values are reset
            distanceTraveled = 0;
            LastPosition = CustomVirtualCursor.GetMousePosition_V2();
            trailTotal = 1;

            //init new trail obj
            Instantiate(trail, transform, worldPositionStays: false);

            //create an countdown to destroy the trail if player doesn't use it in time
            timer = StartCoroutine(trailCountdown());
        }
        
        
    }

    //countdown, 
    private IEnumerator trailCountdown()
    {

        yield return new WaitForSeconds(trailDuration);

        DeleteTrail();
    }


    ///delete the trail, but make sure their is trail to delete first, obv
    public void DeleteTrail()
    {

        if (isTrailActive)
        {
            Destroy(transform.GetChild(0).gameObject);
            isTrailActive = false;
            trailTotal = 0;
            timer = null;
        }
    }


    //return the distance traveld but in the form of how strong the 
    //kb force this should create (out of an average mouse distance movement)
    public float GetDistanceTraveled_Value()
    {

        //the distance to create a circle within the screen for me was about 15 units
        //so
        //set our 'max' to around there (not to high or else player can't get max kb power)
        return distanceTraveled / 5;

    }



    //------------------------------ --------------------------------  -------------------------------





    
}


