
using UnityEngine;
using System;
using Assests.Inputs;


public class Remora_SM : Enemy_ParentClass
{



    /// <summary>
    /// 
    /// Remoras are a more idle type of enemy
    /// 
    /// 
    /// if the plyer mouse collides with this fish then we have to enter stick'd state (ability state)
    /// 
    /// stick'd state, we just slow down the player mouse, and we follow the mouse to make it look like we suck-tion'd 
    /// onto them
    /// 
    /// 
    /// </summary>


    private float colliderSizeY;




    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();


        switch (curr_EnemyState)
        {

            case Enemy_States.idle:

                //swim around the tank
                Idle();
                break;

            case Enemy_States.ability:
                //suck mode, stay on player mouse
                Suck();
                break;
            case Enemy_States.attack:
                //since our parent class sets our base state as attack we reset to idle, 
                //we do this here so we don't end up getting stuck in this state
                curr_EnemyState = Enemy_States.idle;
                break;
            
            //if we add a pet that can remove our stuck state, we need to remove the debuff ***************

            default:
                Debug.Log("Remora has no state");
                break;
        }
    }





    private void Idle()
    {
        float distance = Vector3.Distance(idleTarget, transform.position);

        if (Mathf.Abs(distance) > targetRadius)
        {
            UpdatePosition(idleTarget, idle_velocity);
        }

        //get new point once fish reaches it
        else
        {
            NewRandomIdleTarget_Tank();
        }
    }



    //we should not exit this func once we get stuck to player, unless we create a new pet that does that
    private void Suck()
    {
        //got this from cherry grabb state
        Vector2 pos = CustomVirtualCursor.GetMousePosition_V2();
        pos.y = MathF.Max(pos.y, TankCollision.instance.GetTrashArea().Item4+colliderSizeY); //clamp 
        transform.position = pos;

    }


    public void SetColliderSizeY(float size)
    {
        colliderSizeY = size;
    }



    //if we die we want to re-add our slowdown if we were in our suck state
    protected override void Died()
    {
        base.Died();

        if (curr_EnemyState == Enemy_States.ability)
        {
            Controller_Player.instance.GiveMouseSpdStatusEffect(0.2f);
        }
    } 






    //pretty much the base func, but without the knock back since we dont want this fish getting pushed around
    public override void On_PlayerClick()
    {
        //if the game is paused, return
        if (Controller_EscMenu.instance.paused) { return; }

        //create gun particle
        Controller_Player.instance.Run_GunParticle();

        //damage
        TakeDamage(Controller_Player.instance.Get_GunDamage());
    }


    //this is to start the sticky mode
    //since there is not dedicated on enter sticky mode, im just going to do it here
    public override void On_PlayerEnter(Collider2D collision)
    {
        //if the game is paused, return
        if (Controller_EscMenu.instance.paused) { return; }

        //we also want to check if we are already in the abilty state
        if(curr_EnemyState == Enemy_States.ability) { return; }

        //enter state
        curr_EnemyState = Enemy_States.ability;
        //slow down player mouse
        Controller_Player.instance.GiveMouseSpdStatusEffect(-0.2f);
    }

}
