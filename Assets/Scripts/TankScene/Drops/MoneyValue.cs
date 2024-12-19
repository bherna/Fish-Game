
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoneyValue : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected int moneyValue = 0;
    [SerializeField] float timeTillTrashed = 3f;
    private Rigidbody2D rb;
    [SerializeField] protected AudioClip collectCoinSoundClip;


    private void Start(){

        rb = GetComponent<Rigidbody2D>();
    }


    public void UpdateCoinVal(int newMonVal){
        moneyValue = newMonVal;
    }


    public virtual void OnPointerDown(PointerEventData eventData){

        if(Controller_EscMenu.instance.paused){
            return;
        }

        //add coin
        Controller_Wallet.instance.AddMoney(moneyValue);

        //playsound
        //add a bit of randomness to the pitch to add variance
        var pitch = Random.Range(0.85f, 1.15f);
        AudioManager.instance.PlaySoundFXClip(collectCoinSoundClip, transform, 1f, pitch);
        
        //instantiate text pop up
        Controller_PopUp.instance.CreatePopUp(string.Format("+ {0}",moneyValue));
        //destroy
        Destroy(gameObject);
    }




    //the coin has touched the bottom of the tank, so start self destruct
    public void OnTrashCoin() {
        
        //lock coin position
        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        //event send to pets
        Controller_Pets.instance.Annoucement_Init(Event_Type.coin, gameObject);

        //countdown -> destroy
        IEnumerator coroutine = WaitToDes(timeTillTrashed);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitToDes(float waitTIme){

        yield return new WaitForSeconds(waitTIme);
        Destroy(gameObject);
    }


}
