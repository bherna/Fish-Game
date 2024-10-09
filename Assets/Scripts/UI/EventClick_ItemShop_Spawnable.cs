using UnityEngine;
using TMPro;

public class EventClick_ItemShop_Spawnable : MonoBehaviour
{

    //list of objecive prices, in order of purchase
    [SerializeField] int obj_price = 0;

    //display current item cost on screen
    [SerializeField] TextMeshProUGUI ui_displayCost;

    //what are we selling
    public enum FishType {PlayerFish, Enemy};
    [SerializeField] FishType fishType;

    //prefab fish - we are spawning
    [SerializeField] GameObject fishObj;


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

                case FishType.PlayerFish:

                    //spawn or what ever here
                    //
                    //if we can spawn a fish: pay price
                    if(Controller_Fish.instance.SpawnFish(fishObj, new Vector3(0, 4, transform.position.z))){
                        Wallet.instance.SubMoney(obj_price);

                        //also send an event message to the tutorial
                        Controller_Tutorial.instance.TutorialClick(Expect_Type.Button);
                    }
                    
                    break;

                case FishType.Enemy:
                    Instantiate(fishObj, new Vector3(0, 4, transform.position.z), Quaternion.identity);
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
