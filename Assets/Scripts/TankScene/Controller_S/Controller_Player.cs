using System;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Player : MonoBehaviour
{
    
    [SerializeField] ParticleSystem Gun_particle;
    [SerializeField] GameObject trail;

    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] Transform targetZ;

    public Vector3 mousePos;


    //player stats
    //gun stat
    private int gunPower = 1;


    //counter / trail related
    private float trailDistance = 0;



    //single ton this class
    public static Controller_Player instance {get; private set; }

    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }


    private void Start() {

    }

    private void Update() {

        //get mouse pos
        var screenPos = Input.mousePosition;
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
        mousePos = Camera.main.ScreenToWorldPoint(screenPos); 
        
        //move self there
        //used for collitions
        transform.position = new Vector2(mousePos.x, mousePos.y);

    }


    public void Upgrade_gunPower() {
        gunPower += 1;
    }

    public int Get_GunDamage(){
        return gunPower;
    }


    public void Run_GunParticle(){
        Instantiate(Gun_particle, mousePos, Quaternion.identity);
    }




    //used in counters, create a trail of light that the player needs to return towards the enemy
    //the longer the loop they create, the harder the damage back at the enemy
    public void CreateTrail(){

        //make sure values are reset
        trailDistance = 0;

        //init new trail obj
        //and attach as child
        Instantiate(trail, transform, worldPositionStays:false);
    }


    public void DeleteTrail(){
        Destroy(transform.GetChild(0).gameObject);
    }


}
