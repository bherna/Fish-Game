using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public enum Enemy_States {idle, attack, stunned};

public class Enemy_ParentClass : Fish_ParentClass_Movement, IPointerClickHandler
{
    [SerializeField] protected AudioClip damageSoundClip;
    [SerializeField] protected AudioClip diedSoundClip;
    protected Rigidbody2D rb;
    protected Enemy_States curr_EnemyState;
    

    //targets
    protected Transform currFishTarget;

    
    //stats
    //health should be in # of clicks
    protected int curr_health = 6; 
    protected const float kbForce = 0.7f;



    
    protected new void Start() {
        
        //references
        rb = GetComponent<Rigidbody2D>();

        //idle target set
        NewRandomIdleTarget_Tank();
        
        //attack mode target set
        SetTargetFish(Controller_Fish.instance.GetRandomFish());

        //start in attack mode if possible
        curr_EnemyState = Enemy_States.attack;

    }


    public void SetTargetFish(Transform newFishTarget){

        currFishTarget = newFishTarget;
    }

    
    public void OnPointerClick(PointerEventData eventData){

        //if the game is paused, return
        if(Controller_EscMenu.instance.paused){
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

    private void OnTriggerStay2D(Collider2D other) {
        
        //we're in the boudry, we need to head back
        if(other.gameObject.CompareTag("Boundry")){

            //set our velocity towards middle of tank
            Vector2 kb = (other.gameObject.transform.position - transform.position).normalized * kbForce;
            rb.AddForce(kb, ForceMode2D.Impulse);
            
        }
    }


    protected void OnTriggerEnter2D(Collider2D other) {

        
    }


    //overrideing our original updatePosition
    public bool UpdatePosition(Vector3 target_pos, float current_Vel){

        var dir = (target_pos - transform.position).normalized;

        rb.AddForce(dir * current_Vel * Time.deltaTime, ForceMode2D.Force);

        return true;
    }


    //fish died
    //dont want to flat out destroy object
    protected void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //play sound
        AudioManager.instance.PlaySoundFXClip(diedSoundClip, transform, 1f, 1f);

        //die
        Destroy(gameObject);
    }


    protected new void OnDrawGizmosSelected() {

        switch(curr_EnemyState){
            case Enemy_States.idle:
                //curr idle target
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(idleTarget, 0.5f);
                break;
            case Enemy_States.attack:
                if(currFishTarget == null){return;}
                //current fish target
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(currFishTarget.transform.position, 0.5f);
                break;
        }
        
    }


}
