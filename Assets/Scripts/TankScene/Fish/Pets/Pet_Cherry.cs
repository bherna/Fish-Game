
using System;
using UnityEngine;

public class Pet_Cherry : Pet_ParentClass
{

    [SerializeField] SpriteRenderer sprite; //actual sprite component
    [SerializeField] Sprite closed; //main cherry sprite to use
    [SerializeField] Sprite open; //when pearl is ready use this sprite

    [SerializeField] GameObject pearl;
    private Rigidbody2D rb;

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
        sec_Max = UnityEngine.Random.Range(totalSecForPearl.Item1, totalSecForPearl.Item2);

        //for falling
        rb = GetComponent<Rigidbody2D>();
    }






    //cherry is a special interactable pet, 
    //when enemies are present, cherry can be dropped onto enemies to stun them
    private new void Update()
    {
        base.Update();

        switch (curr_PetState)
        {
            case Pet_States.idle:
                break;

            case Pet_States.grabbed:
                //while grabbed, all we do extra is set our pos to be with mouse
                Vector2 pos = Controller_Player.instance.mousePos;
                pos.y = MathF.Max(pos.y, TankCollision.instance.GetTrashArea().Item4);
                transform.position = pos;
                break;

            case Pet_States.dropped:

                break;
        }



        //also do this always, (maybe might be changed to be everything besides grabbed)
        //moved to own functino for readability
        MakePearl();

    }

    //when cherry collides with enemy, and currnent state is dropped, stun enemy
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")  && curr_PetState == Pet_States.dropped) {
            collision.gameObject.GetComponent<Enemy_ParentClass>().OnStunned(4);
        }
    }






    //three functions to switch between states
    //the only purpose of the states, is to check to see if we are being dropped
    //but to be dropped, we need to be grabbed ...
    //so
    //one from bot of tank, dropped -> idle
    public override void OnTouchGround()
    {
        if(curr_PetState == Pet_States.grabbed){ return; } //else we can go past the trash/botom of tank collider
        curr_PetState = Pet_States.idle;
        //set gravity scale to 0 since we are at bottom
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
    }
    //one from player onmousedown, idle/dropped -> grabbed
    public void OnMouseDown()
    {
        curr_PetState = Pet_States.grabbed;
        //set gravity to 0
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
    }
    // ----------- WE DONT NEED ONE FOR ON DRAG, SINCE WE JUST CARE ABOUT THE DOWN AND UP EVENTS ------------ 
    //one from player onmouseup, grabbed -> dropped
    public void OnMouseUp()
    {
        curr_PetState = Pet_States.dropped;
        //set gravity scale to _ since we are now wanting to fall
        rb.gravityScale = 1;
        rb.velocity = Vector3.zero;
    }


    //don't need this since update does this for us
    /*
    public void OMouseDrag()
    {
        transform.position = Controller_Player.instance.mousePos;
    }
    */


    private void MakePearl()
    {
        if (!pearlReady)
        {

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
                sec_Max = UnityEngine.Random.Range(totalSecForPearl.Item1, totalSecForPearl.Item2);

                //instantiate a pearl
                Vector3 pos = transform.position - Vector3.forward;
                Instantiate(pearl, pos, Quaternion.identity);
            }

        }
    }


    private void PearlCollected()
    {

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
