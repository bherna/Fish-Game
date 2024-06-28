using UnityEngine.EventSystems;
using UnityEngine;

public class Starfish_SM : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] Transform sprite;
    [SerializeField] float startSize = 1;

    [SerializeField] float endSize = 3;

    [SerializeField] float rateOfGrowth = 1;

    [SerializeField] int damageAmount = 20;

    private int health = 4;

    private float currSize = 0;

    private void Start() {
        currSize = startSize;
    }

    // Update is called once per frame
    public void Update()
    {
        if(currSize < endSize){

            currSize += Time.deltaTime * rateOfGrowth;

            sprite.localScale = new Vector3(currSize, currSize, 1);
        }
    }


    public void OnPointerClick(PointerEventData eventData){


        //if the game is paused, return
        if(Controller_Main.instance.paused){
            return;
        }

        //damage
        health -= 5;

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

    private void OnTriggerStay2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Fish")){
            
            other.gameObject.GetComponent<Fish_Stats>().TakeDamage(damageAmount);


        }
    }
}
