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

        //is object affordable
        if(Wallet.instance.IsAffordable(obj_price)){

            switch(fishType){

                case FishType.Goldfish:

                    //spawn or what ever here
                    //
                    //if we can spawn a fish: pay price
                    if(Controller_Fish.instance.SpawnFish()){
                        Wallet.instance.SubMoney(obj_price);
                        //also send an event message to the tutorial
                        UI_Tutorial.instance.Playerclick(Expect_Type.Button);
                    }
                    
                    break;

                case FishType.LargeMBass:
                    Instantiate(fish, new Vector3(0, 4, transform.position.z), Quaternion.identity);
                    break;
                
                default:
                    Debug.Log("no fish selected for this item button.");
                    break;

            }
            
        }
        else{
            
            Debug.Log("Not enough money to buy fish. ");
        }

    }
}
