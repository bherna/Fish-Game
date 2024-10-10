using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Khalid_Stats : FishStats_ParentClass
{
    public override void Died(bool playSound = true){

        //removes self from the list of current fish known to the fish controller
        Controller_Fish.instance.RemoveFish(gameObject);
        
        //play die sound
        if(playSound){AudioManager.instance.PlaySoundFXClip(dieSoundClip, transform, 1f);}
        

        Destroy(gameObject);
    }



    //just reference base class
    public override void TakeDamage(int damage)
    {
        GetComponent<Pet_Khalid>().TakeDamage(damage);
    }
}
