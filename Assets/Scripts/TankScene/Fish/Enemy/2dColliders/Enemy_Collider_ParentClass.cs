
using System;
using UnityEngine;

public abstract class Enemy_Collider_ParentClass : MonoBehaviour
{


    //this should be filled with an actual {enemyName}_collider, not this class
    protected Enemy_ParentClass enemy_ParentClass;


    //init colliders (these might all be different, like one enem might use capsule and another use square)
    protected abstract void Start();





    //purpose of having a tank  coll and a player coll
    //assuming we wan't to help the player with clicking on fast moving enemies
    //having small collision boxes doesn't help

    // to circumvent this issue, we have 2 colliders attached to one enemy object, 
    // on on parent object and one on as a child object





    //tank colliders, this is purely enemy interacting between tank obstacles/fish/anything in the tank
    //  (this doen's exclude player collider)
    //  (this only exludes the player clicking this enemy)


    //tank colliders =>  != player clickcs

    public void OnTriggerEnter2D(Collider2D collision)
    {
        enemy_ParentClass.On_TankEnter(collision); 
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        enemy_ParentClass.On_TankStay(collision); 
    }

    public void OTriggerExit2D(Collider2D collision)
    {
        enemy_ParentClass.On_TankExit(collision); 
    }


}
