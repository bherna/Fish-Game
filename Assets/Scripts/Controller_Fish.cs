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


    // Start is called before the first frame update
    void Start()
    {
        fish_list = new List<GameObject>();


        fish_stages = new List<string>
        {
            "Baby",
            "Teen",
            "Adult"
        };


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
        fish_list[fish_list.Count-1].GetComponent<Fish_SM>().GetTankDem(tankColl);
    }

    public void RemoveFish(GameObject fish){

        fish_list.Remove(fish);

    }


    //return whole fish stages list
    public List<string> GetFishStages(){

        return fish_stages;
    }
}
