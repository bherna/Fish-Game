using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] Controller_Enemy controller_Enemy;

    private Transform currFishTarget;

    [SerializeField] Transform sprite;

    [SerializeField] float velocity = 2f;

    public int damageValue {get; private set; } = 1;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updatePos();
    }


    private void updatePos(){

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
        transform.position = Vector2.MoveTowards(
            transform.position,
            currFishTarget.position,
            velocity * Time.deltaTime
        );

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

    
    
    
}
