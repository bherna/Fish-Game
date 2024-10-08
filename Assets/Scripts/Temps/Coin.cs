
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] int coinValue = 0;
    [SerializeField] float timeTillTrashed = 3f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioClip collectCoinSoundClip;

    public void UpdateCoinVal(int newCoinVal){
        coinValue = newCoinVal;
    }

    public void OnPointerDown(PointerEventData eventData){

        if(Controller_Main.instance.paused){
            return;
        }

        //add coin
        Wallet.instance.AddMoney(coinValue);
        //playsound
        AudioManager.instance.PlaySoundFXClip(collectCoinSoundClip, transform, 1f);
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
