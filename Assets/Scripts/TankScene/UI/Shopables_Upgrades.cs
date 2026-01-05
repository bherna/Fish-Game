using Assests.Inputs;
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
        currUISprite.sprite = sprites[index_array];
    }




    //when button pushed to purchase
    public override void OnPurchase(){

        PrintTransaction();

        switch(upgradeType){

            case Upgrades.foodMax:

                Controller_Food.instance.Upgrade_FoodMax();
                break;

            case Upgrades.foodPower:

                //update sprite + cost
                index_array++;
                currUISprite.sprite = sprites[index_array];

                //upgrade
                Controller_Food.instance.Upgrade_FoodPower();

                //is this last purchase
                if(index_array >= prices.Length-1 ){
                    //disable button, since we reached max
                    GetComponent<Button>().interactable = false;
                    //send to requirement contrller
                    PetReq_ParentClass.instance.MaxFoodReached();
                }
                
                break;

            case Upgrades.FishTotal:
                //????
                Controller_Fish.instance.Upgrade_FishMax();
                break;

            case Upgrades.GunPower:

                Controller_Player.instance.Upgrade_GunPower();
                break;
                
            default:
                Debug.Log("no Upgrade was set...");
                break;
        }
    }

    private void PrintTransaction(){

        //FIRST
        //CAN WE BUY THE shopable
        if(Controller_Wallet.instance.IsAffordable(prices[index_array])){

            //purchase obj
            Controller_Wallet.instance.SubMoney(prices[index_array]);
            //visual
            Controller_PopUp.instance.CreateTextPopUp(string.Format("- {0}", prices[index_array]), CustomVirtualCursor.MousePosition);
            //sound
            Controller_FXSoundsManager.instance.PlaySoundFXClip(buySoundClip, transform, 1f, 1f);
        }
    
    }

    public override void OnPointerEnter(PointerEventData eventData){
        
        string dispString = string.Format("Buy {0} \nCost: {1}", upgradeType, prices[index_array].ToString());
        ToolTip.ShowToolTip(dispString);
    }

    
}
