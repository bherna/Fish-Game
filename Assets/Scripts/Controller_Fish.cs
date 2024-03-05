using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Controller_Fish : MonoBehaviour
{

    [SerializeField] int maxFish = 3;
    [SerializeField] GameObject fishObj;
    [SerializeField] List<GameObject> fish_list;

    [SerializeField] Controller_Food food_c;

    [SerializeField] GameObject tankColl;



    // Start is called before the first frame update
    void Start()
    {
        fish_list = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        
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


    
}
