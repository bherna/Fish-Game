using System;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Player : MonoBehaviour
{
    
    [SerializeField] ParticleSystem Gun_particle;

    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] Transform targetZ;

    [SerializeField] GameObject Gems_Parent;
    public Vector3 mousePos;

    //gems == ammo
    private int curr_gem_count;
    private const int gems_max = 10; //how many gems we start with



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
        curr_gem_count = gems_max;
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
        if(curr_gem_count < 0){Debug.Log("Gem count is going negative."); return;} //
        Gems_Update();
        
    }

    public void Gems_Add(int amount){
        //set max possbile gems first
        int new_gems = curr_gem_count + amount;
        new_gems = Math.Min(new_gems,10);

        curr_gem_count = new_gems;
        Gems_Update();
    }

    //returns true if gems are at max capacity
    public bool Gems_AtMax(){
        return curr_gem_count == gems_max;
    }


    //used for updating gems on ui
    public void Gems_Update(){

        //just set all to deactivated first
        //then activate all gems upto curr_gems
        //(since we need to think about setting gems at start and if player edits transparency

        for(int i = 0; i < gems_max; i++){
            Gems_Parent.transform.GetChild(i).gameObject.SetActive(false);
            Color newTrans = new Color(1,1,1,1);
            Gems_Parent.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Image>().color = newTrans;
        }

        for(int i = 0; i < curr_gem_count; i++){
            Gems_Parent.transform.GetChild(i).gameObject.SetActive(true);
            
        }
    }


    //used for showing how many gems to the player they could get from sacrificing a fish
    public void Gems_Show(int addedGems){

        //set max possbile gems first
        int possible_gems = curr_gem_count + addedGems;
        possible_gems = Math.Min(possible_gems,10);


        //do the same as update, but half the transparency
        for(int i = 0; i < gems_max; i++){
            Gems_Parent.transform.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = 0; i < possible_gems; i++){
            Gems_Parent.transform.GetChild(i).gameObject.SetActive(true);
            Color newTrans = new Color(1,1,1,0.5f);
            Gems_Parent.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Image>().color = newTrans;
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
