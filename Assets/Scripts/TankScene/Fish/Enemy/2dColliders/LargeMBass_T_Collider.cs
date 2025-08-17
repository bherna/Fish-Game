using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeMBass_T_Collider : Enemy_Collider_ParentClass
{
    CapsuleCollider2D capCollider; //we don't reall use this one for this script really


    protected override void Start()
    {
        capCollider = GetComponent<CapsuleCollider2D>();
        enemy_ParentClass = transform.parent.GetComponent<LargeMBass_SM>();

    }

}
