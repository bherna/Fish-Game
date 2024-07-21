using UnityEngine;
using TMPro;

public class EventClick_ItemShop_Spawnable : MonoBehaviour
{

    //list of objecive prices, in order of purchase
    [SerializeField] int obj_price = 0;

    //display current item cost on screen
    [SerializeField] TextMeshProUGUI ui_displayCost;

    //what are we selling
    public enum FishType {Goldfish, LargeMBass};
    [SerializeField] FishType fishType;
    [SerializeField] GameObject fish;


    void Start()
    {
        //display obj cost
        ui_displayCost.text = obj_price.ToString();
    }


    //when button pushed to purchase
    public void OnPurchase(){


        bool buy = false;

        //is object affordable
        if(Wallet.instance.IsAffordable(obj_price)){

            switch(fishType){
                case FishType.Goldfish:
                    //spawn or what ever here
                    if(Controller_Fish.instance.SpawnFish()){
                        buy = true;
                    }
                    
                    break;

                case FishType.LargeMBass:
                    Instantiate(fish, new Vector3(0, 4, transform.position.z), Quaternion.identity);
                    buy = true;
                    break;
                
                default:
                    Debug.Log("no fish selected for this item button.");
                    break;
            }

            if(buy){
                //purchase obj
                Wallet.instance.SubMoney(obj_price);
            }
            
        }
        else{
            
            Debug.Log("Not enough money to buy fish. ");
        }

    }
}
