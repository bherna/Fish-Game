using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Stats : MonoBehaviour
{

    [SerializeField] AudioClip dieSoundClip;
    
    // --------------------------------- Combat related --------------------------------- // 
    private int health;
    private const int maxHealth = 100;


    // ---------------------------------                --------------------------------- //

    protected void Start(){
        health = maxHealth;
    }


    public void TakeDamage(int damage){

        health -= damage;

        if(health <= 0){

            Died();

        }
    }


    
    public void Died(bool playSound = true){

        //removes self from the list of current fish known to the fish controller
        Controller_Fish.instance.RemoveFish(gameObject);
        
        //play die sound
        if(playSound){AudioManager.instance.PlaySoundFXClip(dieSoundClip, transform, 1f);}
        

        Destroy(gameObject);
    }

}
