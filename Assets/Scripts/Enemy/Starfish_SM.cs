using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class Starfish_SM : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] Transform sprite;
    private int damageAmount = 20;
    private float attackSpeed = 0.3f; //per second
    private bool canAttack = true;

    private int health = 8; // in terms of clicks


    // Update is called once per frame
    public void Update()
    {

    }


    public void OnPointerClick(PointerEventData eventData){


        //if the game is paused, return
        if(Controller_Main.instance.paused){
            return;
        }

        //damage
        health -= Controller_Player.instance.Get_GunDamage();

        //die
        if(health <= 0){
            Died();
        }
        
    }

    //fish died
    //will be more than one way to die
    private void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //die
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        if(canAttack && other.gameObject.CompareTag("Fish")){
            
            other.gameObject.GetComponent<Fish_Stats>().TakeDamage(damageAmount);

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
