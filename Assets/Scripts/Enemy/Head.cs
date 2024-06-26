using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{

    //bite-ing fish vars
    private int attackPower = 20; 
    private float attackSpeed = 0.7f; //per second
    private bool canAttack = true;

    [SerializeField] LargeMBass_SM bodyScript;


    private void OnTriggerStay2D(Collider2D other) {
        
        if(canAttack && other.gameObject.CompareTag("Fish")){
            
            //bite
            other.gameObject.GetComponent<Fish_Stats>().TakeDamage(attackPower);

            //reset vel
            bodyScript.ResetVelocity();
 
            //now wait for next bite
            canAttack = false;
            IEnumerator co = AttackCooldown();
            StartCoroutine(co);

        }
    }


    private IEnumerator AttackCooldown() {

        yield return new WaitForSeconds(1/attackSpeed);
        canAttack = true;
    }
}
