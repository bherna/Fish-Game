using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pet_Cherry : Pet_ParentClass, IPointerClickHandler
{

    [SerializeField] SpriteRenderer sprite;

    //pearl production var's
    private float sec_tillPearl = 0;
    private const float totalSecForPearl = 36f;
    private bool pearlReady = false;
    private int pearlValue = 225;




    // Start is called before the first frame update
    private new void Start()
    {
        //dont any of the movement stuff, so no base.start() method


    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();

        if(!pearlReady){
            
            //build up pearl
            sec_tillPearl += Time.deltaTime;

            if(sec_tillPearl > totalSecForPearl){
                //read to click
                pearlReady = true;
                //open mouth animation
                sprite.color = Color.red;
                //play sound
            }

        }

    }


    public void OnPointerClick(PointerEventData eventData){

        //if the game is paused, return
        if(Controller_EscMenu.instance.paused){
            return;
        }

        if(pearlReady){
            //Debug.Log(string.Format("Clicked Cherry"));

            //collect pearl money
            Controller_Wallet.instance.AddMoney(pearlValue);
            
            //reset variables
            sec_tillPearl = 0;
            pearlReady = false;

            //update animation to closed
            sprite.color = Color.white;
        }
    }


    public override void Event_Init(Event_Type type, GameObject obj){}

    public override void Event_EndIt(Event_Type type){}
}
