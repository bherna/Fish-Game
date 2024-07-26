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
    [SerializeField] protected int curr_health = 10; 
    [SerializeField] protected float velocity = 0f;
    [SerializeField] protected float kbForce = 0f;
    

    

    protected void updatePos(){

        //head towards target 
        var newVector = (currFishTarget.position - transform.position).normalized;
        rb.AddForce(newVector * velocity, ForceMode2D.Force);

        //fliping
        if(transform.position.x - currFishTarget.position.x < 0){
            sprite.localScale = new Vector3(-1f, 1f, 1f);
            
        }
        else{
            sprite.localScale = new Vector3(1f, 1f, 1f);
        }

    }

    protected void SetTargetFish(Transform newFishTarget){

        currFishTarget = newFishTarget;
    }

    
    public void OnPointerClick(PointerEventData eventData){


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
    //will be more than one way to die
    protected void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //die
        Destroy(gameObject);
    }


    
}
