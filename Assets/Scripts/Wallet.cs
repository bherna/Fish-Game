using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wallet : MonoBehaviour
{

    //current money
    int current_money = 0;

    //reference to self
    public static Wallet instance {get; private set; }

    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }   

    
    public void AddMoney(int money){

        current_money += money;
    }

    public void SubMoney(int money){
        current_money -= money;
    }

    public bool IsAffordable(int price){
        return price < current_money;
    }


}
