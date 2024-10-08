using System;
using System.Collections.Generic;
using UnityEngine;


public class Controller_Food : MonoBehaviour
{

    //----------------------- references ---------------------------
    [SerializeField] GameObject[] foodPellets;
    private int index_foodPelletType = 0;
    
    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] public Transform targetZ;

    //audios
    [SerializeField] AudioClip createSound;
    [SerializeField] AudioClip destroySound;


    // -------------------------------- privates --------------------------------
    private int maxFood = 3;
    private List<GameObject> foodPellets_list;  
    





    //singleton this class
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
        //start empy array for food
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

            //delete money if we have money
            if(Wallet.instance.IsAffordable(5)){Wallet.instance.SubMoney(5);}
            
            //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //orthagraphic
            var screenPos = Input.mousePosition;
            screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
            var mousePos = Camera.main.ScreenToWorldPoint(screenPos); 
            SpawnFood_Pellet(mousePos);
        }
    }




    //used for spawning player placed food pellet
    private void SpawnFood_Pellet(Vector3 mousePos){
        
        //check if at max food,
        //delete oldest
        FoodList_MakeSpace();

        //spawn new
        foodPellets_list.Add(Instantiate(foodPellets[index_foodPelletType], mousePos, Quaternion.identity));

    }



    //used for spawning non conventional food pellet (like pet created foods ex: burger)
    //this new food pellet should be created before hand, all we do here is make space in tank and add to foodlist
    public void SpawnFood_Gameobject(GameObject food_obj){

        //checkk if at max food,
        //delete oldest
        FoodList_MakeSpace();

        //add given gameobject to list
        foodPellets_list.Add(food_obj);
    }


    //used in keeping track of pellet count in the tank
    //if we are currently at max food capacity in tank,
    //          delete the oldest food (which is first index)
    private void FoodList_MakeSpace(){

        if(foodPellets_list.Count >= maxFood){
            Destroy(foodPellets_list[0]);
            foodPellets_list.RemoveAt(0);

            //play sound to know we destroy'd food
            //badd sound effect
            AudioManager.instance.PlaySoundFXClip(destroySound, transform, 1f);
        }
        else{
            //play good sound effect
            AudioManager.instance.PlaySoundFXClip(createSound, transform, 1f);
        }
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

    //upgrades food power
    //also returns true if the array is finished
    public void Upgrade_FoodPower(){
        //increment array index
        index_foodPelletType++;
    }

    
}
