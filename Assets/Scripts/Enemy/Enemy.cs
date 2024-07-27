using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class Enemy : MonoBehaviour, IPointerClickHandler
{


    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform sprite;
    

    //targets
    protected Transform currFishTarget;
    

    //stats
    //health should be in # of clicks
    [SerializeField] protected int curr_health = 6; 
    [SerializeField] protected float kbForce = 0f;
    



    protected void SetTargetFish(Transform newFishTarget){

        currFishTarget = newFishTarget;
    }

    
    public void OnPointerClick(PointerEventData eventData){

        //make sure we have ammo to use (one gem click)
        if( ! Controller_Player.instance.Gems_Available(1)){
            Debug.Log("Not Enough gems to attack.");
            return; //not enough gems to click
        }
        else{
            //use 1 gem (for now)
            Controller_Player.instance.Gems_Sub(1);
        }


        //if the game is paused, return
        if(Controller_Main.instance.paused){
            return;
        }

        //create gun particle
        Controller_Player.instance.Run_GunParticle();

        //damage
        curr_health -= Controller_Player.instance.Get_GunDamage();

        //knockback
        var kbVector = new Vector2(
                transform.position.x - eventData.pointerCurrentRaycast.worldPosition.x,
                transform.position.y - eventData.pointerCurrentRaycast.worldPosition.y
            ).normalized;

        rb.AddForce(kbVector * kbForce, ForceMode2D.Impulse);

        //die
        if(curr_health <= 0){
            Died();
        }
        
    }


    //fish died
    //dont want to flat out destroy object
    protected void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //die
        Destroy(gameObject);
    }


    
}
