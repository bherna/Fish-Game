using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IPointerClickHandler
{


    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform sprite;
    

    //targets
    private Transform currFishTarget;
    

    //stats
    public int damageValue {get; private set; } = 1;
    public int health;
    private const int maxHealth = 20; 

    [SerializeField] float velocity = 1f;
    [SerializeField] float kbForce = 1f;
    



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if(Controller_Main.instance.paused){
            return;
        }

        //if no fish currently pointing to
        if(currFishTarget == null){
            
            //if so get a new fish to follow
            SetTargetFish(Controller_Fish.instance.GetRandomFish());

            //if we can't find a fish, just return
            if(currFishTarget == null){

                //wait a few seconds before checking again ?
                return;
                
            }
            
        }

        else{
            //update the enemy position towards target fish
            updatePos();
        }
        
    }


    private void updatePos(){

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

    public void SetTargetFish(Transform newFishTarget){

        currFishTarget = newFishTarget;
    }

    
    public void OnPointerClick(PointerEventData eventData){


        //if the game is paused, return
        if(Controller_Main.instance.paused){
            return;
        }

        //damage
        health -= 5;

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
    private void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //die
        Destroy(gameObject);
    }



    
    
}
