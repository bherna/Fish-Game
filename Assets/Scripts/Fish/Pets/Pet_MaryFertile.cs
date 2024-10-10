using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
//    Desc: A pregnat fish lady named Mary Fertile, who loves to have as many children as possible
//    Ability:  Every _ number of seconds Mary will spawn in a new guppy into the tank
//              The guppy itself is just a normal guppy, nothing crazy,
//              - Mary does not spawn guppys during enemy wave mode
//    Rarity: Level Questing
//
/// </summary>
/// 


public class Pet_MaryFertile : Pet_ParentClass
{

    private Event_Type event_type = Event_Type.enemyWave;
    private float curr_secToGuppy = 0;
    private float secondsTillGuppy = 5f; //_ seconds
    private bool inEnemyWave = false;
    
    [SerializeField] private GameObject guppy_ref; 




    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update(); //incase

        IdleMode(); //movement
        
        //--------- enemy wave related stuff ------------ (returns at this point)


        curr_secToGuppy += Time.deltaTime;

        if (inEnemyWave){return;}

        if(curr_secToGuppy >= secondsTillGuppy){
            //BABY TIMEEEEEEEEEEEEE
            Controller_Fish.instance.SpawnFish(guppy_ref, transform.position);
            //reset
            curr_secToGuppy = 0;
        }



        
    }



    //start of enemy wave event
    public override void Event_Init(Event_Type type, GameObject obj)
    {
        if(type != event_type){return;}
        inEnemyWave = true;
    }

    //end of enemy wave event
    public override void Event_EndIt(Event_Type type)
    {
        if(type != event_type){return;}
        inEnemyWave = false;
    }

}
