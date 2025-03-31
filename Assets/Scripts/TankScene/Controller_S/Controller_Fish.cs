using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Fish : MonoBehaviour
{

    //max fish to have in the tank at a given time
    [SerializeField] int maxFish = 50;

    //list of current fish in tank
    [SerializeField] List<GameObject> fish_list;



    //singleton this class
    public static Controller_Fish instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }




    // Start is called before the first frame update
    void Start()
    {

        //create empty fish list
        fish_list = new List<GameObject>();
    }

    public bool SpawnFish(GameObject fishObj, Vector3 vec_pos){

        //Debug.Log("Fish Spawned");

        //spawn new fish if max is not reached
        if(fish_list.Count >= maxFish){

            Debug.Log("max fish reached");
            return false;
        }

        //spawn at top of tank
        fish_list.Add(Instantiate(fishObj, vec_pos, Quaternion.identity));

        //return success
        return true;    
    }
    

    //for all guppies in the tank, we increment 1 age stage, so baby -> teen and teen -> adults
    public void AddAge(){

        foreach(GameObject guppy in fish_list){
            guppy.GetComponent<Guppy_Stats>().Fish_Birthday();
        }
    }


    //does what its called
    public void RemoveFish(GameObject fish){

        fish_list.Remove(fish);

    }


    //gets a reference to one of the guppies we have in the tank
    //this is useful for enemies to randomly choose a prey
    public Transform GetRandomFish(){

        //if fish list is empty
        if(fish_list.Count == 0){
            return null;
        }

        var rand = UnityEngine.Random.Range(0, fish_list.Count-1);

        return fish_list[rand].transform;
    }


    public int GetFishCount(){
        return fish_list.Count;
    }


    public void Upgrade_fishMax(){
        maxFish += 1;
    }


    //----------------------  pet related -------------------------------------
    public void PetEvent_Huddle(GameObject pet){
        foreach(GameObject fish in fish_list){
            fish.GetComponent<Guppy_SM>().GuppyToFollow(pet);
        }
    }
    public void PetEvent_Disperse(){
        foreach(GameObject fish in fish_list){
            fish.GetComponent<Guppy_SM>().GuppyToIdle();
        }
    }



    ////---------------------- tutorial ------------------------------------------
    public void TutorialEvent_GuppysNowCanEat(){
        foreach(GameObject fish in fish_list){
            if(fish.tag == "Tutorial"){
                fish.GetComponent<Guppy_Stats_Tutorial>().GuppyCanEatNow();
            }
        }
    }
    public void TutorialEvent_GuppysNowCanAge(){
        foreach(GameObject fish in fish_list){
            if(fish.tag == "Tutorial"){
                fish.GetComponent<Guppy_Stats_Tutorial>().GuppyCanAgeNow();
            }
        }
    }
    
}
