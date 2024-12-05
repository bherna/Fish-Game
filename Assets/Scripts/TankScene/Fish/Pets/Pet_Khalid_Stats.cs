using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_Khalid_Stats : FishStats_ParentClass
{

    private new void Start() {
        maxHealth = 5;
        health = maxHealth;
    }
    public override void Died(bool playSound = true){

        GetComponent<Pet_Khalid>().DiedStats();
        
        //change enemy target away from us
        Controller_Enemy.instance.GetEnemyAtIndex(0).GetComponent<Enemy_ParentClass>().SetTargetFish(Controller_Fish.instance.GetRandomFish());
        
        //play 'die' sound, transisioning to polyp mode
        if(playSound){AudioManager.instance.PlaySoundFXClip(dieSoundClip, transform, 1f, 1f);}
        
    }



    //just reference base class
    public override void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0){
            Died();
        }
        
    }

    public void ResetHealth(){
        health = maxHealth;
    }
}
