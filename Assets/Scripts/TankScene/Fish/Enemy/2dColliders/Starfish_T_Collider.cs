using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfish_T_Collider : Enemy_Collider_ParentClass
{
    // Start is called before the first frame update
    protected override void Start()
    {
        enemy_ParentClass = transform.parent.GetComponent<Starfish_SM>();
    }


    void OnMouseDown()
    {

    }
    
    //nothing different from base parent class really
}
