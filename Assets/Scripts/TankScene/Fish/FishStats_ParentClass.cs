using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishStats_ParentClass : MonoBehaviour
{
    // --------------------------------- health/combat related -------------------------------------------//
    [SerializeField] protected AudioClip dieSoundClip;
    protected int health;
    protected int maxHealth = 7;    // health is in terms of number of bites (health = _ # of bites before we die)
                                            //for enemy (number of clicks till death) (health = _# of clicks before we die)
    
    protected void Start(){
        health = maxHealth;
    }


    public abstract void TakeDamage(int damage);

    public abstract void Died(bool playSound = true);
}
