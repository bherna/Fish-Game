
using System;
using UnityEngine;

public abstract class Enemy_Collider_ParentClass : MonoBehaviour
{

    //collider types:
    //  -all: access all collider types from this script alone
    //  -player: acces only Ipointerclickhandler logic
    //  -tank: access only collider2d logic
    public enum ColliderType { All, Player, Tank };

    //this should be filled with an actual {enemyName}_collider, not this class
    protected Enemy_ParentClass enemy_ParentClass;
    [SerializeField] protected ColliderType colliderType;


    //init colliders (these might all be different, like one enem might use capsule and another use square)
    protected abstract void Start();





    //purpose of having a tank  coll and a player coll
    //assuming we wan't to help the player with clicking on fast moving enemies
    //having small collision boxes doesn't help

    // to circumvent this issue, we have 2 colliders attached to one enemy object, 
    // each collider is attached to an empty child object
    // to differ between each collider, we have the [collidertype] data type

    //on trigger ENTER, this is the initial touching of enemy coll with player coll
    //we dont really expect player to pixel perfect click on enter
    // so we only check for player collider on trigger STAY






    public void OnTriggerEnter2D(Collider2D collision)
    {
        enemy_ParentClass.On_TankEnter(collision);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        
        switch (colliderType)
        {
            case ColliderType.Player:
                enemy_ParentClass.On_PlayerClick();
                break;

            case ColliderType.Tank:
                enemy_ParentClass.On_TankStay(collision);
                break;

            default:
                enemy_ParentClass.On_PlayerClick();
                enemy_ParentClass.On_TankStay(collision);
                break;
        }
   
        
    }


}
