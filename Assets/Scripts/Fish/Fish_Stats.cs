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

            GetComponent<Fish_SM>().Died();

        }
    }


    


}
