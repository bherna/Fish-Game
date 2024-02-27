using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick_Coin : MonoBehaviour, IPointerClickHandler
{
    private Coin coin;

    private void Awake(){

        coin = GetComponent<Coin>();
    }

    public void OnPointerClick(PointerEventData eventData){

        Destroy(gameObject);
    }
}
