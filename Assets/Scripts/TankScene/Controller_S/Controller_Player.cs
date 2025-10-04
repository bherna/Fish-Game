using System;
using System.Collections;

using UnityEngine;


public class Controller_Player : MonoBehaviour
{

    [SerializeField] ParticleSystem Gun_particle;
    [SerializeField] GameObject trail;
    
    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] Transform targetZ;




    //since this is public, if any ui elemnt needs it position when clicked, we can use this
    public Vector3 mousePos = new Vector3(0,0,0);


    //player stats
    //gun stat
    private int gunPower = 1;
    public int cursorSpeed_base = 45; //player will be able to adjust this in the settings, Adjust this value
    public int cursorSpeed_curr = 45; //what is used in game



    //counter / trail related
    private float distanceTraveled = 0;
    private bool isTrailActive = false;
    public const int trailDuration = 1; //in seconds how long we can counter for(this is used in enemy code aswell to keep in sync)
    private Vector2 LastPosition;
    private int trailTotal = 0; // this keeps track of current chained trails
    private Coroutine timer;


    //used for clamping mousepos on screen
    public Vector3 max { get; private set; }
    public Vector3 min { get; private set; }

    




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
        //set up mouse variables, im not sure what it all means tho ;p
        float v3 = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
        Camera cam = Camera.main;


        

       
        max = cam.ViewportToWorldPoint(new Vector3(1, 1, v3));
        min = cam.ViewportToWorldPoint(new Vector3(0, 0, v3));

    }

    private void Update()
    {
/*
        //get mouse pos + make sure not to leave the screen
        var screenPos = Input.mousePosition;
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);

        mousePos = Camera.main.ScreenToWorldPoint(screenPos); //convert to world pos
        mousePos.x = Mathf.Clamp(mousePos.x, min.x, max.x); //clamp
        mousePos.y = Mathf.Clamp(mousePos.y, min.y, max.y); //clamp



        //collider (since coll is attached to gameobject)
        transform.position = new Vector2(mousePos.x, mousePos.y);
        //if we need to use transfomr.position, just know its not the tank area (WARNING)
*/

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 moveDelta = new Vector3(mouseX, mouseY, 0) * cursorSpeed_curr * Time.deltaTime;
        transform.position += moveDelta;

        


        //if we have a trail active, we want to update our distance traveld
        if (isTrailActive)
        {
            //update distance counter
            Vector2 newPosition = mousePos;
            distanceTraveled += (newPosition - LastPosition).magnitude;
            LastPosition = newPosition;
        }

    }


    //this is used to slow down player mouse from debuffs,
    //give a percentage value of how much the debuff slows the player mouse down
    public void MouseStat(float percentage)
    {
        //like Value = 100% - percentage;
        cursorSpeed_curr = cursorSpeed_base + Mathf.FloorToInt(percentage * cursorSpeed_base);

        Math.Clamp(cursorSpeed_curr, 0, cursorSpeed_base);
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
        Instantiate(Gun_particle, mousePos, Quaternion.identity);
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
            LastPosition = mousePos;
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


