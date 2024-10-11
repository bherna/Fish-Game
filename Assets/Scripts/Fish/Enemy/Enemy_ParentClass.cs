using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class Enemy_ParentClass : Fish_ParentClass_Movement, IPointerClickHandler
{


    [SerializeField] protected AudioClip damageSoundClip;
    [SerializeField] protected AudioClip diedSoundClip;
    protected Rigidbody2D rb;
    

    //targets
    protected Transform currFishTarget;
    

    //stats
    //health should be in # of clicks
    [SerializeField] protected int curr_health = 6; 
    [SerializeField] protected float kbForce = 0f;
    
    protected void Start() {
        rb = GetComponent<Rigidbody2D>();
    }


    public void SetTargetFish(Transform newFishTarget){

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
    //dont want to flat out destroy object
    protected void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //play sound
        AudioManager.instance.PlaySoundFXClip(diedSoundClip, transform, 1f);

        //die
        Destroy(gameObject);
    }


    
}
