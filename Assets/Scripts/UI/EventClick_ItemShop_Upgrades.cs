using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventClick_ItemShop_Upgrades : MonoBehaviour
{

    //list of objecive prices, in order of purchase
    [SerializeField] int[] prices;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image ui_sprite;
    private int index_array = 0;


    //display current item cost on screen
    [SerializeField] TextMeshProUGUI ui_displayCost;


    //types of upgrads
    public enum Upgrades {foodMax, foodPower, FishTotal, GunPower};

    //what are we upgrading
    [SerializeField] Upgrades upgradeType;




    void Start()
    {
        //are the lists the same size
        if(prices.Length != sprites.Length){
            Debug.Log("Lists are not the same size");

        }

        //display obj cost
        ui_displayCost.text = prices[index_array].ToString();
        //sprite
        ui_sprite.sprite = sprites[index_array];
    }


    //when button pushed to purchase
    public void OnPurchase(){

        //is object affordable
        if(Wallet.instance.IsAffordable(prices[index_array])){

            //purchase obj
            Wallet.instance.SubMoney(prices[index_array]);
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

                //update sprite + cost
                index_array++;
                ui_sprite.sprite = sprites[index_array];
                ui_displayCost.text = prices[index_array].ToString();

                //upgrade
                Controller_Food.instance.Upgrade_FoodPower();

                //is this last purchase
                if(index_array >= prices.Length-1 ){
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
