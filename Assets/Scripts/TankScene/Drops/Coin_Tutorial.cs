using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin_Tutorial : Coin
{

    //override original to message controller tutorial


    public override void OnPointerDown(PointerEventData eventData){

        if(Controller_EscMenu.instance.paused){
            return;
        }

        //add coin
        Controller_Wallet.instance.AddMoney(moneyValue);
        //playsound
        AudioManager.instance.PlaySoundFXClip(collectCoinSoundClip, transform, 1f, 1f);


        //controller tutorial
        Controller_Tutorial.instance.CollectCoin();

        //destroy
        Destroy(gameObject);
    }
}
