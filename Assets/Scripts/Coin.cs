using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 10;

    private void OnDestroy() {
        
        Wallet.instance.AddMoney(coinValue);
    }
}
