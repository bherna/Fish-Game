using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Main : MonoBehaviour
{

    //start money for current level
    [SerializeField] int startMoney;


    private void Start() {
        
        Wallet.instance.AddMoney(startMoney);
    }

    
}
