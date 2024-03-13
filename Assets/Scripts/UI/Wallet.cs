using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Wallet : MonoBehaviour
{

    //current money
    int current_money = 0;

    //post current money
    [SerializeField] TextMeshProUGUI ui_text;

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


        UpdateMoney();
    }   

    
    public void AddMoney(int money){

        current_money += money;
        UpdateMoney();
    }

    public void SubMoney(int money){
        current_money -= money;
        UpdateMoney();
    }

    public bool IsAffordable(int price){
        return price <= current_money;
    }


    private void UpdateMoney(){

        ui_text.text = current_money.ToString();
    }


}
