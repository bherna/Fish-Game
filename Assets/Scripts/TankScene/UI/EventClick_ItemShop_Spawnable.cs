using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] GameObject guppyPrefab;
    [SerializeField] GameObject guppyPrefab_tutorial;


    void Start()
    {
        //display obj cost
        ui_displayCost.text = obj_price.ToString();
    }

    void OnEnable()
    {
        //Register Button Events
        GetComponent<Button>().onClick.AddListener(() => OnPurchase());
    }

    void OnDisable()
    {
        //Un-Register Button Events
        GetComponent<Button>().onClick.RemoveAllListeners();
    }


    //when button pushed to purchase
    public void OnPurchase(){

        //is object affordable
        if(Controller_Wallet.instance.IsAffordable(obj_price)){

            switch(fishType){

                case FishType.PlayerFish:

                    //spawn 
                    //first make sure we arn't in tutorial mode
                    if(Controller_Tutorial.instance.tutorial_active){
                        //spawn tutorial version
                        if(Controller_Fish.instance.SpawnFish(guppyPrefab_tutorial, new Vector3(0, 4, transform.position.z))){
                            Controller_Wallet.instance.SubMoney(obj_price);
                        }
                    }
                    else{
                        //normal mode
                        //if we can spawn a fish: pay price
                        if(Controller_Fish.instance.SpawnFish(guppyPrefab, new Vector3(0, 4, transform.position.z))){
                            Controller_Wallet.instance.SubMoney(obj_price);
                        }
                    }
                    
                    break;

                case FishType.Enemy:
                    Instantiate(guppyPrefab, new Vector3(0, 4, transform.position.z), Quaternion.identity);
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
