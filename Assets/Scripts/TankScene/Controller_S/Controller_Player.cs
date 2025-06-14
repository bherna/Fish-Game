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

        //get mouse pos
        var screenPos = Input.mousePosition;
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
        mousePos = Camera.main.ScreenToWorldPoint(screenPos);

        //move self there
        //used for collisions
        transform.position = new Vector2(mousePos.x, mousePos.y);

        //click stuff
        if (Input.GetMouseButtonDown(0))
        {
            PlayerMouseDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PlayerMouseUp();
        }


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



    //------------------------------ this is for mouse presses code  -------------------------------


    //for mouse presses, we are going to rely on a collider circle, whenever player mouse hovers over an in-game object
    // ie: fish, enemy, drops, (clcickables really)
    // we are goinng to enter an expect to click mode

    //when player does, click down -> set our bool to true (interact bool)
    //when collision enters, other object is gonna check to see if their is an interact bool available
    //if yes, absorb

    //this will reset to false again, if a click up event happens, or timer runs out (lasts for 1.4 frames)
    private bool click = false;
    Coroutine timer;

    //other scripts use this one to check if we have a click
    public bool GetPlayerClick()
    {
        bool temp = click;
        click = false;
        return temp;
        
    }

    //ran in update method
    private void PlayerMouseDown()
    {
        //set to true
        click = true;
        //remove any old timer going off
        if (timer != null)
        {
            StopCoroutine(timer);
        }
        //start timer to reset click
        timer = StartCoroutine(RunTimer());
    }
    //ran in update method
    private void PlayerMouseUp()
    {
        //reset click to false again
        click = false;

        //stop timer
        if (timer != null)
        {
            StopCoroutine(timer);
        }
    }

    private IEnumerator RunTimer()
    {
        float secondsLeft = 1.5f;

        while (secondsLeft > 0)
        {
            secondsLeft -= 1;
            yield return new WaitForFrames(1);
        }

        click = false;
    }

    //------------------------------ --------------------------------  -------------------------------







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


//used in helping with creating the click mech
//(credit dgoyette)
public class WaitForFrames : CustomYieldInstruction
{
    private int _targetFrameCount;

    public WaitForFrames(int numberOfFrames)
    {
        _targetFrameCount = Time.frameCount + numberOfFrames;
    }

    public override bool keepWaiting
    {
        get
        {
            return Time.frameCount < _targetFrameCount;
        }
    }
}