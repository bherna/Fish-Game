using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller_Food : MonoBehaviour
{

    //spawn food pellets for the tank with mouse R click
    [SerializeField] int maxFood = 3;

    [SerializeField] GameObject foodPellet_basic;

    [SerializeField] List<GameObject> foodPellets_list;

    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] public Transform targetZ;

    


    //static variable for fish coin value
    public static Controller_Food instance {get; private set; }
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
        foodPellets_list = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        //is the game currently paused
        if(Controller_Main.instance.paused){
            return;
        }

        //spawn pellet
        if(Input.GetMouseButtonDown(1)){
            
            //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //orthagraphic
            var screenPos = Input.mousePosition;
            screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
            var mousePos = Camera.main.ScreenToWorldPoint(screenPos); 
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








    /// FUNCTIONS FOR OTHER SCRIPTS TO CALL 
    public List<GameObject> GetAllFood(){
        return foodPellets_list;
    }

    public void TrashThisFood(GameObject foodObj){

        foodPellets_list.Remove(foodObj);
        Destroy(foodObj);
    }

    public int GetFoodLength(){
        return foodPellets_list.Count;
    }

    public void Upgrade_foodMax(){
        maxFood += 1;
    }

    public void Upgrade_FoodPower(){
        //increment index of food list or something
    }

    
}
