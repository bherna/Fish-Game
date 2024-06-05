
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick_Coin : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] int coinValue = 0;
    [SerializeField] float timeTillTrashed = 1.5f;
    [SerializeField] Rigidbody2D rb;

    public void UpdateCoinVal(int newCoinVal){
        coinValue = newCoinVal;
    }

    public void OnPointerDown(PointerEventData eventData){

        if(Controller_Main.instance.paused){
            return;
        }

        Wallet.instance.AddMoney(coinValue);
        Destroy(gameObject);
    }


    public void OnTrashCoin() {
        
        //lock coin position
        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        //countdown -> destroy
        IEnumerator coroutine = WaitToDes(timeTillTrashed);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitToDes(float waitTIme){

        yield return new WaitForSeconds(waitTIme);
        Destroy(gameObject);
    }


}
