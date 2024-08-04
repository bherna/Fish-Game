using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventClick_ItemShop_Upgrades : MonoBehaviour
{

    //list of objecive prices, in order of purchase
    [SerializeField] int obj_price = 0;

    //display current item cost on screen
    [SerializeField] TextMeshProUGUI ui_displayCost;


    //types of upgrads
    public enum Upgrades {foodMax, foodPower, FishTotal, GunPower};

    //what are we upgrading
    [SerializeField] Upgrades upgradeType;




    void Start()
    {
        //display obj cost
        ui_displayCost.text = obj_price.ToString();
    }


    //when button pushed to purchase
    public void OnPurchase(){

        //is object affordable
        if(Wallet.instance.IsAffordable(obj_price)){

            //purchase obj
            Wallet.instance.SubMoney(obj_price);
        }
        else{
            
            Debug.Log("Not enough money to buy upgrade: " + upgradeType.ToString());
            return;
        }

        switch(upgradeType){

            case Upgrades.foodMax:
                Controller_Food.instance.Upgrade_foodMax();
                break;

            case Upgrades.foodPower:

                if(Controller_Food.instance.Upgrade_FoodPower()){
                    //disable button, since we reached max
                    GetComponent<Button>().interactable = false;
                }

                break;

            case Upgrades.FishTotal:

                Controller_Fish.instance.Upgrade_fishMax();
                break;

            case Upgrades.GunPower:
            
                Controller_Player.instance.Upgrade_gunPower();
                break;
                
            default:
                Debug.Log("no Upgrade was set...");
                break;
        }
    }
}
