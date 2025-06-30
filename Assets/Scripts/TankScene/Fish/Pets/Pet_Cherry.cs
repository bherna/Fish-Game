using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Cherry : Pet_ParentClass
{

    [SerializeField] SpriteRenderer sprite; //actual sprite component
    [SerializeField] Sprite closed; //main cherry sprite to use
    [SerializeField] Sprite open; //when pearl is ready use this sprite

    [SerializeField] GameObject pearl;

    //event calling event type
    private Event_Type event_type = Event_Type.PearlCollected;

    //pearl production var's
    private float sec_tillPearl = 0; //current count in seconds
    private (float, float) totalSecForPearl = (36f, 70f);
    private float sec_Max = 0; //what amount of time we plan on waiting for a pearl
    private bool pearlReady = false;




    // Start is called before the first frame update
    private new void Start()
    {
        //dont any of the movement stuff, so no base.start() method

        //update sec_Max
        sec_Max= Random.Range(totalSecForPearl.Item1, totalSecForPearl.Item2);
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();

        if(!pearlReady){
            
            //build up pearl
            sec_tillPearl += Time.deltaTime;

            if (sec_tillPearl > sec_Max)
            {
                //read to click
                pearlReady = true;

                //open mouth animation
                sprite.sprite = open;

                //play ready sound
                //--------------------

                //update next sec_max 
                sec_Max = Random.Range(totalSecForPearl.Item1, totalSecForPearl.Item2);

                //instantiate a pearl
                Vector3 pos = transform.position - Vector3.forward;
                Instantiate(pearl, pos, Quaternion.identity);
            }

        }

    }


    private void PearlCollected(){
        
        //Debug.Log(string.Format("Clicked Cherry"));

        //reset variables
        sec_tillPearl = 0;
        pearlReady = false;

        //update animation to closed
        //sprite.color = Color.white;
        sprite.sprite = closed;
    }


    public override void Event_Init(Event_Type type, GameObject obj)
    {
        
        if (type == event_type)
        {
            PearlCollected();
        }
    }

    public override void Event_EndIt(Event_Type type){}
}
