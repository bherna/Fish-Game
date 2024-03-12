using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick_Coin : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] int coinValue = 0;

    /*
    public void OnPointerClick(PointerEventData eventData){

        Wallet.instance.AddMoney(coinValue);
        Destroy(gameObject);
    }
    */
    public void UpdateCoinVal(int newCoinVal){
        coinValue = newCoinVal;
    }

    public void OnPointerDown(PointerEventData eventData){

        Wallet.instance.AddMoney(coinValue);
        Destroy(gameObject);
    }
}
