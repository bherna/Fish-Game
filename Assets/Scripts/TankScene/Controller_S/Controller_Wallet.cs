using TMPro;
using UnityEngine;

public class Controller_Wallet : MonoBehaviour
{

    //current money
    private int current_money;

    //post current money
    [SerializeField] TextMeshProUGUI ui_text;





    //reference to self
    public static Controller_Wallet instance {get; private set; }

    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }

    }   





    private void Start() {

        current_money = GameVariables.GetStartMoney();
        UpdateMoney();
        //Debug.Log("Gamvar: "+GameVariables.GetStartMoney());
        //Debug.Log("Our start money: "+current_money);
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
