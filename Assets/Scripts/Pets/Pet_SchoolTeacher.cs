using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_SchoolTeacher : Pet_ParentClass
{
    

    //School Teacher pet
    // ability :
    //          When ever a new enemy wave starts, 
    //          The school teacher will call all guppies in tank to itself
    //          and keep them huddled untill the enemies are all gone

    //          The school teacher will try to avoid the enemies as much as
    //          possible. 


    private new void Start() {
        base.Start(); //still start init variables from parent class

        
    }


    //when the enemy wave starts, this pet will enter protect mode
    //protect mode is school teacher's ability
    private void ProtectMode(){

    }


}
