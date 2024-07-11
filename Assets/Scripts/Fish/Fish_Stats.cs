using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Stats : MonoBehaviour
{
    
    private int health = 0;
    private const int maxHealth = 100;

    private void Start() {
        
        health = maxHealth;

    }


    public void TakeDamage(int damage){

        health -= damage;

        if(health <= 0){

            Died();

        }
    }

    public void Died(){

        Controller_Fish.instance.RemoveFish(gameObject);

        Destroy(gameObject);
    }

    


}
