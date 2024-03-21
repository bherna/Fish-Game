using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class Enemy : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] Controller_Enemy controller_Enemy;
    [SerializeField] Rigidbody2D rb;

    private Transform currFishTarget;

    [SerializeField] Transform sprite;

    [SerializeField] float velocity = 1f;

    public int damageValue {get; private set; } = 1;

    public int health;
    private const int maxHealth = 20; 
    [SerializeField] float forceMod = 1f;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        updatePos();
    }


    private void updatePos(){

        //is game paused, (need to pause fish, since they repeatedly get free force when unpaused
        if(Controller_Main.instance.paused){
            return;
        }

        //did our fish get destroyed
        if(currFishTarget == null){
            
            //if so get a new fish to follow
            SetTargetFish(controller_Enemy.GetRandomFish());

            //if we can't find a fish, just return
            if(currFishTarget == null){

                //wait a few seconds before checking again
                StartCoroutine(waitSomeSeconds(5f));
                return;
            }
            
        }

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

    public void SetController_Enemy(Controller_Enemy enemy_c){
        controller_Enemy = enemy_c;
    }

    public IEnumerator waitSomeSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
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
        //Debug.Log("vector: " + kbVector.ToString());
        rb.AddForce(kbVector * forceMod, ForceMode2D.Impulse);

        //die
        if(health <= 0){
            Destroy(gameObject);
        }
        
    }
    
    
}
