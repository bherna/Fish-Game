using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shopables_Upgrades : Shopables_ParentClass, IPointerEnterHandler, IPointerExitHandler
{

    //list of objecive prices, in order of purchase
    [SerializeField] int[] prices;
    [SerializeField] Sprite[] sprites;
    private int index_array = 0;



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

        //update sprite
        currSprite.sprite = sprites[index_array];
    }




    //when button pushed to purchase
    public override void OnPurchase(){

        //FIRST
        //CAN WE BUY THE shopable
        if(Controller_Wallet.instance.IsAffordable(prices[index_array])){

            //purchase obj
            Controller_Wallet.instance.SubMoney(prices[index_array]);
            //visual
            Controller_PopUp.instance.CreatePopUp(string.Format("- {0}", prices[index_array]));
        }
        else{
            
            Debug.Log("Not enough money to buy upgrade: " + upgradeType.ToString());
            return;
        }
        //if not then return and null a null a null a dlfsaj

        switch(upgradeType){

            case Upgrades.foodMax:
                Controller_Food.instance.Upgrade_foodMax();
                break;

            case Upgrades.foodPower:

                //update sprite + cost
                index_array++;
                currSprite.sprite = sprites[index_array];

                //upgrade
                Controller_Food.instance.Upgrade_FoodPower();

                //is this last purchase
                if(index_array >= prices.Length-1 ){
                    //disable button, since we reached max
                    GetComponent<Button>().interactable = false;
                }
                
                break;

            case Upgrades.FishTotal:
                //????
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

    public override void OnPointerEnter(PointerEventData eventData){
        
        string dispString = string.Format("Buy {0} \nCost: {1}", upgradeType, prices[index_array].ToString());
        ToolTip.ShowToolTip(dispString);
    }

    
}
