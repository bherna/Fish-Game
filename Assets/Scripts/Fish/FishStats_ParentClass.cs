using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishStats_ParentClass : MonoBehaviour
{
    // --------------------------------- health/combat related -------------------------------------------//
    [SerializeField] protected AudioClip dieSoundClip;
    protected int health;
    protected const int maxHealth = 100;
    
    protected void Start(){
        health = maxHealth;
    }


    public abstract void TakeDamage(int damage);

    public abstract void Died(bool playSound = true);
}
