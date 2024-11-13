using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public enum Enemy_States {idle, attack};

public class Enemy_ParentClass : Fish_ParentClass_Movement, IPointerClickHandler
{

    [SerializeField] protected BoxCollider2D attach_pos;
    [SerializeField] protected AudioClip damageSoundClip;
    [SerializeField] protected AudioClip diedSoundClip;
    protected Rigidbody2D rb;
    protected Enemy_States curr_EnemyState = Enemy_States.idle;
    

    //targets
    protected Transform currFishTarget;

    //retargeting variables, used in idle mode
    protected float currSeconds_ReTarget = 0;
    protected float secondsTill_ReTarget = 3;

    
    

    //stats
    //health should be in # of clicks
    [SerializeField] protected int curr_health = 6; 
    [SerializeField] protected float kbForce = 0f;
    
    protected void Start() {
        
        //references
        rb = GetComponent<Rigidbody2D>();
        SetTargetFish(Controller_Fish.instance.GetRandomFish());

        //idle 
        NewRandomIdleTarget_Tank();
        startTime = Time.time;
        curr_EnemyState = Enemy_States.attack;

        targetRadius = 0.8f;

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

    //move around the tank
    //get a random point on the screen
    protected void IdleMode(){

        //retargeting 
        currSeconds_ReTarget += Time.deltaTime;
        if(currSeconds_ReTarget >= secondsTill_ReTarget){
            currSeconds_ReTarget = 0;
            curr_EnemyState = Enemy_States.attack;
            return;
        }

        //else we keep idle mode
        float distance;
        if(attach_pos != null){
            distance = Vector3.Distance(new Vector3(idleTarget.x - attach_pos.offset.x, idleTarget.y - attach_pos.offset.y, 0), transform.position);
        }
        else{
            distance = Vector3.Distance(idleTarget, transform.position);
        }

        if(Mathf.Abs(distance) > targetRadius){
            
            updatePosition(idleTarget, idle_velocity);
        }

        //get new point once fish reaches it
        else{
            NewRandomIdleTarget_Tank();
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


    protected new void OnDrawGizmosSelected() {
    
        base.OnDrawGizmosSelected();

        if(currFishTarget == null || currFishTarget.transform.position  == new Vector3(0,0,0)){
            //dont show
        }
        else{
            //current range untill new target
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(currFishTarget.transform.position, 0.5f);
        }
        
    }
}
