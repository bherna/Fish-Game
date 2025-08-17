using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//when a guppy dies, this object / class gets instantiated, while the original guppy gets deleted
    //the purpose of this dead guppy is for the player to clean up their tank, since dead guppys dont
    //jsut magically disappear
    
    
    
public class Drop_Money_GuppyDead : Drop_Money
{




    public override void OnTrashDrop()
    {

        //we just want to keep chilling at the bottom of the tank
        //taken from drop_parent:
        //lock coin position
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
}
