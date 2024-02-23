using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller_Food : MonoBehaviour
{

    //spawn food pellets for the tank with mouse R click
    [SerializeField] int maxFood = 3;

    [SerializeField] GameObject foodPellet_basic;

    [SerializeField] List<GameObject> foodPellets_list;

    // Start is called before the first frame update
    void Start()
    {
        foodPellets_list = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        //spawn pellet
        if(Input.GetMouseButtonDown(1)){
            
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            SpawnFoodPellet(mousePos);
        }
    }


    private void SpawnFoodPellet(Vector3 mousePos){
        
        //check if at max food,
        //delete oldest
        if(foodPellets_list.Count >= maxFood){
            Destroy(foodPellets_list[0]);
            foodPellets_list.RemoveAt(0);
        }

        //spawn new
        foodPellets_list.Add(Instantiate(foodPellet_basic, mousePos, Quaternion.identity));

    }
}
