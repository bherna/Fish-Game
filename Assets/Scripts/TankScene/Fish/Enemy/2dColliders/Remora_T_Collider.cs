using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remora_T_Collider : Enemy_Collider_ParentClass
{

    //basic square for both i think
    BoxCollider2D coll;


    // Start is called before the first frame update
    protected override void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        enemy_ParentClass = transform.parent.GetComponent<Remora_SM>();
    }
}
