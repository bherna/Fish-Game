using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remora_P_Collider : MonoBehaviour
{

    private Remora_SM remora_sm;
    private BoxCollider2D coll;


    protected void Start()
    {
        remora_sm = GetComponent<Remora_SM>();
        coll = GetComponent<BoxCollider2D>();

        //send our collider size over to sm since its needed for sticky mode
        remora_sm.SetColliderSizeY(coll.size.y);
    }

    //not normal player click event, read the largemouth bass version of this function

    public void OnMouseDown()
    {
        remora_sm.On_PlayerClick();
    }


    //also need one on collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        remora_sm.On_PlayerEnter(collision); 
    }
}
