using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Wallet : MonoBehaviour
{

    //current money
    private int current_money;
    //post current money for ui
    [SerializeField] TextMeshProUGUI ui_text;



    //this is to keep track of income
    //but income is calculated in a combo format, 
    //if the player keeps continously collecting money fast enough 'income' will go up
    //if they stop after _ seconds, income -> 0
    private int income;
    //we also need timer stuff
    //both in seconds
    private int countDownTimer = 0;
    private const int timerStart = 4;





    //reference to self
    public static Controller_Wallet instance { get; private set; }

    private void Awake()
    {

        //delete duplicate of this instance

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }




    /// ------------------------------------------ base functions ----------------------------------------------

    private void Start()
    {
        //set our start money for this level
        current_money = LocalLevelVariables.GetStartMoney();
        UpdateMoney();
    }


    public void AddMoney(int money)
    {

        current_money += money;
        UpdateMoney();

        AddToCombo(money);
    }

    public void SubMoney(int money)
    {
        current_money -= money;
        UpdateMoney();
    }

    public bool IsAffordable(int price)
    {
        return price <= current_money;
    }

    //used for updating the total money player has (store ui)
    private void UpdateMoney()
    {
        ui_text.text = current_money.ToString();
    }



    //----------------------------------------------------- combo related -------------------------------------

    //the function every obj will reference
    //all they need to think about is adding to the combo
    public void AddToCombo(int amt)
    {

        //if we have no combo 
        if (income == 0)
        {
            //start new combo
            countDownTimer = timerStart;
            income = amt;
            //update teext 
            UI_Combo.instance.UpdateText(income.ToString());
            //start countdown
            StartCoroutine(CountDown());
        }
        else if (income >= 1)
        {

            //add to combo level
            income += amt;
            UI_Combo.instance.UpdateText(income.ToString());
            //reset countdown
            countDownTimer = timerStart;
        }
    }



    private IEnumerator CountDown()
    {

        while (countDownTimer > 0)
        {

            //wait for 1 second to pass
            yield return new WaitForSeconds(1);
            //sub
            countDownTimer -= 1;
            Debug.Log("while loop pass -1");


            //update pet-req text
            PetReq_ParentClass.instance.SetIncome(income);

        }


        //else we lost the combo 
        //reset
        countDownTimer = 0;
        income = 0;
        UI_Combo.instance.UpdateText("0");
        PetReq_ParentClass.instance.SetIncome(0);

    }



}
