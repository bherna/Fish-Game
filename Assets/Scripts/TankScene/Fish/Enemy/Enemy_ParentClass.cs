using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public enum Enemy_States {idle, attack};

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
    private int curr_health = 6; 
    private float kbForce = 5f;



    
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

    public bool UpdatePosition(Vector3 target_pos, float current_Vel){

        var dir = Vector3.MoveTowards( transform.position, target_pos, current_Vel * Time.deltaTime );

        rb.AddForce(dir, ForceMode2D.Force);

        return true;
    }

    //move around the tank
    //get a random point on the screen
    protected void IdleMode(){

        float distance = Vector3.Distance(idleTarget, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            UpdatePosition(idleTarget, idle_velocity);
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

        switch(curr_EnemyState){
            case Enemy_States.idle:
                //curr idle target
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(idleTarget, 0.5f);
                break;
            case Enemy_States.attack:
                //current fish target
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(currFishTarget.transform.position, 0.5f);
                break;
        }
        
    }


}
