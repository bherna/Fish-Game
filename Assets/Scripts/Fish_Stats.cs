using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Stats : MonoBehaviour
{
    
    [SerializeField] int health = 0;
    [SerializeField] const int maxHealth = 100;

    [SerializeField] Controller_Fish controller_Fish;

    private void Start() {
        
        health = maxHealth;

        controller_Fish = gameObject.GetComponent<Fish_SM>().GetControllerFish();
    }


    public void TakeDamage(int damage){

        health -= damage;

        if(health <= 0){

            Died();

        }
    }

    public void Died(){

        controller_Fish.RemoveFish(gameObject);

        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Enemy")){
            
            var component = other.gameObject.GetComponent<Enemy>();

            TakeDamage(component.damageValue);


        }
    }


}
