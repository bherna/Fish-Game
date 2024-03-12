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

    [SerializeField] GameObject tankColl;



    //static variable for fish coin value
    public static Controller_Fish instance {get; private set; }

    [SerializeField] List<string> fish_stages;


    //tank demensions
    private float tank_xLower = 0;
    private float tank_xUpper = 0;
    private float tank_yLower = 0;
    private float tank_yUpper = 0;


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
        GetTankDem();


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


    public void SpawnFish(){

        //spawn new fish if max is not reached
        if(fish_list.Count >= maxFish){

            Debug.Log("max fish reached");
            return;
        }

        //spawn at top of tank
        fish_list.Add(Instantiate(fishObj, new Vector2(0, 4), Quaternion.identity));
        fish_list[fish_list.Count-1].GetComponent<Fish_SM>().SetFoodController(food_c);
        fish_list[fish_list.Count-1].GetComponent<Fish_SM>().SetFishController(this);
        fish_list[fish_list.Count-1].GetComponent<Fish_SM>().SetTankDem(tank_xLower, tank_xUpper, tank_yLower, tank_yUpper);
    }

    public void RemoveFish(GameObject fish){

        fish_list.Remove(fish);

    }


    //return whole fish stages list
    public List<string> GetFishStages(){

        return fish_stages;
    }



    public void GetTankDem(){

        var tank_size = tankColl.GetComponent<BoxCollider2D>().size;
        var w = tank_size.x;
        var h = tank_size.y;

        var tank_pos = tankColl.transform.position;

        tank_xLower = tank_pos.x - w/2;
        tank_xUpper = tank_pos.x + w/2;

        tank_yLower = tank_pos.y - h/2;
        tank_yUpper = tank_pos.y + h/2;

       
    }

    public Transform GetRandomFish(){

        //if fish list is empty
        if(fish_list.Count == 0){
            return null;
        }

        var rand = Random.Range(0, fish_list.Count-1);

        return fish_list[rand].transform;
    }
}
