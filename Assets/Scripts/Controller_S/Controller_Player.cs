using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    
    [SerializeField] ParticleSystem Gun_particle;

    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] Transform targetZ;

    [SerializeField] GameObject Gems_Parent;
    public Vector3 mousePos;

    //gems == ammo
    private int curr_gem_count;
    private const int gem_start = 10; //how many gems we start with



    //player stats
    //gun stat
    private int gunPower = 1;





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

        //set our gems interal + ui
        curr_gem_count = gem_start;
        Gems_Update();

    }

    private void Update() {

        //get mouse pos
        var screenPos = Input.mousePosition;
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
        mousePos = Camera.main.ScreenToWorldPoint(screenPos); 
        
        //move self there
        transform.position = new Vector2(mousePos.x, mousePos.y);
    }



    //returns true if the parameter amount passed is less then / equal to gems we have in stock
    public bool Gems_Available(int amount){

        return amount <= curr_gem_count;
    }

    public void Gems_Sub(int amount){
        curr_gem_count -= amount;
        Gems_Update();
    }

    public void Gems_Add(int amount){
        curr_gem_count += amount;
        Gems_Update();
    }
    //used for updating gems on ui
    private void Gems_Update(){

        //just set all to deactivated first
        //then activate all gems upto curr_gems
        //(since we need to think about setting gems at start)

        for(int i = 0; i < gem_start; i++){
            Gems_Parent.transform.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = 0; i < curr_gem_count; i++){
            Gems_Parent.transform.GetChild(i).gameObject.SetActive(true);
        }
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






}
