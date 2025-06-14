using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Collider_ParentClass : MonoBehaviour
{

    //this should be filled with an actual {enemyName}_collider, not this class
    protected Enemy_ParentClass enemy_ParentClass;


    //init colliders (these might all be different, like one enem might use capsule and another use square)
    protected abstract void Start(); 
    




    //For all enemy colliders, we are going to seperate the collider2d attachment from main parent object into its own sub/child object that interacts
    // with main enemy-sm code, each enemy should have their own {enemyname}_collider script to differ the ways they interact with tank

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        enemy_ParentClass.Coll_OnStay(other);
    }



    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        enemy_ParentClass.Coll_OnTrigger(other);
    }
}
