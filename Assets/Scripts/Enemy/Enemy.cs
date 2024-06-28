using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class Enemy : MonoBehaviour, IPointerClickHandler
{


    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;
    

    //targets
    protected Transform currFishTarget;
    

    //stats
    public int health;
    private const int maxHealth = 20; 

    [SerializeField] float velocity = 1f;
    [SerializeField] float kbForce = 1f;
    



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    

    protected void updatePos(){

        //head towards target 
        var newVelocity = (currFishTarget.position - transform.position).normalized;
        rb.AddForce(newVelocity * velocity, ForceMode2D.Force);

        //sprite fliping
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

        //damage
        health -= Controller_Main.instance.Get_GunDamage();

        //knockback
        var kbVector = new Vector2(transform.position.x - eventData.pointerCurrentRaycast.worldPosition.x, transform.position.y - eventData.pointerCurrentRaycast.worldPosition.y).normalized;
        rb.AddForce(kbVector * kbForce, ForceMode2D.Impulse);

        //die
        if(health <= 0){
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
