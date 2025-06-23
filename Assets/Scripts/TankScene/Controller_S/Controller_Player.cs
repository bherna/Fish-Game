using System;
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Player : MonoBehaviour
{

    [SerializeField] ParticleSystem Gun_particle;
    [SerializeField] GameObject trail;

    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] Transform targetZ;


    //since this is public, if any ui elemnt needs it position when clicked, we can use this
    public Vector3 mousePos;




    //player stats
    //gun stat
    private int gunPower = 1;


    //counter / trail related
    private float distanceTraveled = 0;
    private bool isTrailActive = false;
    public const int trailDuration = 1; //in seconds (this is used in enemy code aswell to keep in sync)
    private Vector2 LastPosition;


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
        float v3 = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
        Camera cam = Camera.main;


        max = cam.ViewportToWorldPoint(new Vector3(1, 1, v3));
        min = cam.ViewportToWorldPoint(new Vector3(0, 0, v3));

    }

    private void Update()
    {

        //get mouse pos
        //but make sure not to leave the screen
        var screenPos = Input.mousePosition;
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
        mousePos = Camera.main.ScreenToWorldPoint(screenPos);

        mousePos.x = Mathf.Clamp(mousePos.x, min.x, max.x);
        mousePos.y = Mathf.Clamp(mousePos.y, min.y, max.y);


        //move self there
        //used for collisions
        transform.position = new Vector2(screenPos.x, screenPos.y);


        //if we have a trail active, we want to update our distance traveld
        if (isTrailActive)
        {

            Vector2 newPosition = transform.position;
            distanceTraveled += (newPosition - LastPosition).magnitude;
            LastPosition = newPosition;
        }

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
            return;
        }

        isTrailActive = true;

        //make sure values are reset
        distanceTraveled = 0;
        LastPosition = transform.position;

        //init new trail obj
        //and attach as child
        Instantiate(trail, transform, worldPositionStays: false);

        //create an countdown to destroy the trail if player doesn't use it in time
        StartCoroutine(trailCountdown());
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


