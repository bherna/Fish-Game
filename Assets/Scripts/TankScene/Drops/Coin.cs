
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] protected int coinValue = 0;
    [SerializeField] float timeTillTrashed = 3f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] protected AudioClip collectCoinSoundClip;
    [SerializeField] protected GameObject textPopUp;




    public void UpdateCoinVal(int newCoinVal){
        coinValue = newCoinVal;
    }


    public virtual void OnPointerDown(PointerEventData eventData){

        if(Controller_EscMenu.instance.paused){
            return;
        }

        //add coin
        Controller_Wallet.instance.AddMoney(coinValue);

        //playsound
        //add a bit of randomness to the pitch to add variance
        var pitch = Random.Range(0.85f, 1.15f);
        AudioManager.instance.PlaySoundFXClip(collectCoinSoundClip, transform, 1f, pitch);
        
        //instantiate text pop up
        var popup = Instantiate(textPopUp, transform.position, Quaternion.identity);
        popup.GetComponent<TextPopUp>().UpdateText(string.Format("+ {0}",coinValue));
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
