using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{

    [SerializeField] int current_money;

    // Start is called before the first frame update
    void Start()
    {
        current_money = 0;
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
