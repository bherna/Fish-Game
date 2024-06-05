using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Fish : MonoBehaviour
{

    //max fish to have in the tank at a given time
    [SerializeField] int maxFish = 3;
    //prefab
    [SerializeField] GameObject fishObj;
    //list of current fish in tank
    [SerializeField] List<GameObject> fish_list;



    //references to in-game objects
    [SerializeField] Controller_Food food_c;



    //static variable for fish coin value
    public static Controller_Fish instance {get; private set; }

    [SerializeField] List<string> fish_stages;


    //tank demensions
    private float swim_xLower;
    private float swim_xUpper;
    private float swim_yLower;
    private float swim_yUpper;




    // Start is called before the first frame update
    void Start()
    {

        //create empty fish list
        fish_list = new List<GameObject>();


        //fish stages list
        fish_stages = new List<string>
        {
            "Baby",
            "Teen",
            "Adult"
        };

        //update fish tank demensions
        (swim_xLower, swim_xUpper, swim_yLower, swim_yUpper) = TankCollision.instance.GetTankSwimSpawnArea();


    }

    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }


    public Boolean SpawnFish(int fishPrice){

        //is the fish affordable
        if(!Wallet.instance.IsAffordable(fishPrice)){

            Debug.Log("Not enough money to buy fish. ");
            return false;
        }
        else{
            //purchase fish to spawn
            Wallet.instance.SubMoney(fishPrice);
        }

        //spawn new fish if max is not reached
        if(fish_list.Count >= maxFish){

            Debug.Log("max fish reached");
            return false;
        }

        //spawn at top of tank
        fish_list.Add(Instantiate(fishObj, new Vector3(0, 4, transform.position.z), Quaternion.identity));
        fish_list[fish_list.Count-1].GetComponent<Fish_SM>().SetFoodController(food_c);
        fish_list[fish_list.Count-1].GetComponent<Fish_SM>().SetFishController(this);
        fish_list[fish_list.Count-1].GetComponent<Fish_SM>().SetTankSwimDimensions(swim_xLower, swim_xUpper, swim_yLower, swim_yUpper);

        //return success
        return true;    
    }

    public void RemoveFish(GameObject fish){

        fish_list.Remove(fish);

    }


    //return whole fish stages list
    public List<string> GetFishStages(){

        return fish_stages;
    }



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
}
