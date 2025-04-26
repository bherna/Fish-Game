using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Wallet : MonoBehaviour
{

    //current money
    private int current_money;

    //used in determining total money earned per second
    private int income;

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

        current_money = LocalLevelVariables.GetStartMoney();
        UpdateMoney();
        //Debug.Log("Gamvar: "+GameVariables.GetStartMoney());
        //Debug.Log("Our start money: "+current_money);
    }

    
    public void AddMoney(int money){

        current_money += money;
        UpdateMoney();

        income += money;
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



    //this function is used in determining the money made per seccond
    //only calculate the income during requirements phase, since we don't need it right now outside of that
    public IEnumerator CalculateIncome(){

        while(PetReq_ParentClass.instance.toggle){

            //calculate new income (just set money earned as new income)
            PetReq_ParentClass.instance.SetIncome(income);

            //reset values
            income = 0;

            //do again after second
            yield return new WaitForSeconds(1);
            
        }

    }


    


}
