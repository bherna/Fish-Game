using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Salt : Food_Parent
{
    

    //this class ontop of doing parent class
    //sends an event to pet_salt when we get eaten/destroyed
    //this lets salt to throw next food
    private void OnDestroy() {
        
        Controller_Pets.instance.Annoucement_Init(Event_Type.saltDestroyed, null);
    }

}
