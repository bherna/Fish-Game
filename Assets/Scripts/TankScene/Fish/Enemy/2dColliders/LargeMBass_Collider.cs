using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeMBass_Collider : Enemy_Collider_ParentClass
{
    CapsuleCollider2D capCollider;


    protected override void Start()
    {
        capCollider = GetComponent<CapsuleCollider2D>();
        enemy_ParentClass = transform.parent.GetComponent<LargeMBass_SM>();

    }
}
