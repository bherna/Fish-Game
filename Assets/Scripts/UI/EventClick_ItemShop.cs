using UnityEngine;
using TMPro;

public class EventClick_ItemShop : MonoBehaviour
{

    //list of objecive prices, in order of purchase
    [SerializeField] int obj_price = 0;

    //display current item cost on screen
    [SerializeField] TextMeshProUGUI ui_displayCost;


    // Start is called before the first frame update
    void Start()
    {
        //display obj cost
        ui_displayCost.text = obj_price.ToString();
    }


    //when button pushed to purchase
    public void OnPurchase(){

        Controller_Fish.instance.SpawnFish(obj_price);
    }
}
