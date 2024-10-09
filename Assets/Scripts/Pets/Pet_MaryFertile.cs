using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_MaryFertile : Pet_ParentClass
{

    private Event_Type event_type = Event_Type.enemyWave;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    //start of enemy wave event
    public override void Event_Init(Event_Type type, GameObject obj)
    {
        if(type != event_type){return;}


    }

    //end of enemy wave event
    public override void Event_EndIt(Event_Type type){}

}
