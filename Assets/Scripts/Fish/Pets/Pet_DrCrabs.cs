
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
//    Desc: A crab that loves money, but also loves making the best burgers fish can eat
//    Ability:  If you let coins settle at the bottom of the tank, Dr. Crabs will take them. After collecting enough coins, 
//              he will make a burger that will instantly grow a fish to the next age stage.
//    Rarity: Level Questing
//
/// </summary>
/// 
[System.Serializable]
public class coinStack{
    [SerializeField] public List<GameObject> _coins;

    public coinStack(){
        _coins = new List<GameObject>();
    }
}


public class Pet_DrCrabs : Pet_ParentClass
{

    [SerializeField] GameObject burger;
    [SerializeField] protected coinStack cointargets;                              //stack list holding all possible food targets
    private GameObject currTarget_Position;       //dr. crabs current position heading towards (used for collecting coins)
    private Event_Type event_type = Event_Type.coin;

    private float ability_velocity = 2;
    private int curr_coins_collected = 0;
    private int coins_till_burger = 6;
    public float throwStrength = 100;





    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

        cointargets = new coinStack();
        
    }

    // Update is called once per frame
    private new void Update() {
        base.Update();


        //idle when not currently in enemy wave
        //else enter protect mode
        switch(curr_PetState){

            case Pet_States.idle:
                IdleMode();
                break;
            case Pet_States.ability:
                CoinMode();
                break;
            default:
                Debug.Log(gameObject.ToString() + " pet is not in a current state");
                break;
        }

    }


    //Dr. Crabs walks only at the bottom of the tank.
    //he also doesn't really have a state machine, besides just continuously looking for coins in tank
    
    //dr crabs needs to have an updated random tank target position, else we swmin around tank, instead of walking on bottom of tank
    protected override void NewRandomIdleTarget_Tank(){

        //since new target
        NewTargetVariables();

        //new target
        var curr_pos = new Vector3 (transform.position.x, transform.position.y, 0);

        //tanke dememsions
        var swimDem = TankCollision.instance.GetTankSwimArea();

        while(Mathf.Abs(Vector2.Distance(idleTarget, curr_pos)) < newTargetMinLengthRadius){
            
            idleTarget = new Vector3(
                Random.Range(swimDem.Item1, swimDem.Item2),
                -4, 
                0
            );
        }
    }



    //when in coin mode, we want to just follow possilbe food targets in our list
    //once list is empty, enter idle mode again
    private void CoinMode(){

        try{
            //try to update our position based on game object, if the object is missing, throw exception
            updatePosition(currTarget_Position.transform.position, ability_velocity); 
        }
        catch(MissingReferenceException){ //new target then
            NewCoinTarget();
        }
        
    }


    //when ever we get a message for coins, enter coin mode and append food to list targets
    public override void Event_Init(Event_Type type, GameObject food_obj){

        if(type != event_type){return;}
        //we should enter coin mode
        curr_PetState = Pet_States.ability;

        //update food target object, (compare new food to current food target)
        if( currTarget_Position == null ||
            Vector2.Distance(food_obj.transform.position, transform.position) < Vector2.Distance(currTarget_Position.transform.position, transform.position) 
            ){
                
                //then update food obj as new food target, since smaller distance to cover
                //and push into our stack for later use
                NewTargetVariables();
                currTarget_Position = food_obj;
            } 

        //finally push to stack
        cointargets._coins.Add(food_obj);

    } 
    //
    public override void Event_EndIt(Event_Type type){} //do nothing



    private void OnTriggerStay2D(Collider2D other){

        //              FOOD
        //if dr. crabs collides with a food
        if(other.gameObject.CompareTag("Money"))
        {
            //eat + destroy obj
            curr_coins_collected += 1;
            Controller_Food.instance.TrashThisFood(other.gameObject);


            //check if we got enough coins
            if(curr_coins_collected >= coins_till_burger){  // --------------------------- burger created -------------------- //
                //reset collection
                curr_coins_collected = 0; 
                //create burger
                var new_burger = Instantiate(burger, transform.position, Quaternion.identity); 
                //when burger created, its throw into the air
                new_burger.GetComponent<Rigidbody2D>().AddForce(new Vector3(0,throwStrength,0));
                //add burger to food list, so fish can see it
                Controller_Food.instance.AddFood_Gameobject(new_burger, false);
            }


            //now we also update our new current target position
            NewCoinTarget();

        }
    }


    private void NewCoinTarget(){

        //call new variables
        NewTargetVariables();

        //update stack, get next coin that is closest to dr. crabs
        GameObject[] coin_array = cointargets._coins.ToArray();
        Vector2 smallest_distance = new Vector2(10000000, 1000000); 
        GameObject smallest_obj = null;

        //new stack for non empties
        cointargets = new coinStack();

        //while getting next element, make sure target still exists, and add to stack
        //then if not smaller distance than current coin, ignore
        //
        foreach(GameObject coin in coin_array){
            
            if(coin != null){
                cointargets._coins.Add(coin);

                if(Vector2.Distance(coin.transform.position, transform.position) < Vector2.Distance(smallest_distance, transform.position)){
                    smallest_distance = coin.transform.position;
                    smallest_obj = coin;
                }
            }
        }

        //if our cointargets list is empty after this null checking
        //enter idle mode again
        if(cointargets._coins.Count < 1){
            ToIdle();
        }
        else{
            //set new smallest distance to get to
            currTarget_Position = smallest_obj;
        }
        

    }

    //returning to idle variables
    private void ToIdle(){

        curr_PetState = Pet_States.idle;
        currTarget_Position = null;     //rest to null, else pointer issue

        //every time we return to idle, we should keep track of how many times we try to return, 
        //so it doesn't look like dr. crabs is just stuck trying to reach the edge of the tank over and over again.
        NewRandomIdleTarget_Tank();
    }

}
