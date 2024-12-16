using System.Collections;
using Steamworks;
using UnityEngine;




//the object this is attached to is 'a golden ball of light'
//what it does is spawn where the tutorial guppy died  (which should be expected from controller_tutorial)
//then head to the middle of the screen, then turn into money

public class Tutorial_InstanceMoney : MonoBehaviour
{

    private int moneyToAdd = 20;
    [SerializeField] private GameObject textPopUp;
    private Vector3 origin;
    private bool neverPlayed = true;

    void Start(){

        //set origin and currnet z to be infront of ripple layer
        origin = new Vector3(0,0, Controller_Ripple.instance.GetZ()-0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y, origin.z);
        
        //start children ps
        //since we cant start only the children on awake, cringe
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

        var distance = Vector3.Distance(origin, transform.position);
        //first we head to middle of screen
        if(Mathf.Abs(distance) > 1){
            
            transform.position = Vector3.MoveTowards( transform.position, origin, 1 * Time.deltaTime );
        }
        else if(neverPlayed){


            //once at middle of screen
            //turn of children ps
            for(int i = 0; i < transform.childCount; i++){
                transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
            }

            //we explode and turn into money
            //first we play our ps
            GetComponent<ParticleSystem>().Play();

            //then we set a timer to selfdestruck
            //since we can't get the duration, just do 0.2, make it look delayed
            StartCoroutine(SelfDestruct(0.2f));

            //so we dont spam this
            neverPlayed = false;
        }
    }

    private IEnumerator SelfDestruct(float seconds){

        yield return new WaitForSeconds(seconds);

        //instantiate text pop up
        var popup = Instantiate(textPopUp, transform.position, Quaternion.identity);
        popup.GetComponent<TextPopUp>().UpdateText(string.Format("+ {0}", moneyToAdd));
        Controller_Wallet.instance.AddMoney(moneyToAdd);
        //destroy obj
        Destroy(gameObject);
    }
}
