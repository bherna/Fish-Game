using UnityEngine;
using UnityEngine.EventSystems;


public enum FishType {Guppy, Enemy};



public class Shopables_Spawnable : Shopables_ParentClass, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] int fishPrice = 0;

    //what are we selling
    [SerializeField] FishType fishType = FishType.Guppy;

    //prefab fish - what are we spawning
    [SerializeField] GameObject guppyPrefab;
    [SerializeField] GameObject guppyPrefab_tutorial; //if this is the tutorial tank, we use tutorial fish version



    


    //when button pushed to purchase
    public override void OnPurchase(){

        //is object affordable
        if(Controller_Wallet.instance.IsAffordable(fishPrice)){


            switch(fishType){

                case FishType.Guppy:
                    //spawn 

                    //NOW make sure we arn't in tutorial mode
                    if(Controller_Tutorial.instance.sell_tutorial_fish){
                        //spawn tutorial version
                        //if we can spawn a fish: pay price
                        if(Controller_Fish.instance.SpawnFish(guppyPrefab_tutorial, new Vector3(0, 4, transform.position.z))){
                            Controller_Wallet.instance.SubMoney(fishPrice);
                            PrintTransaction();
                        }
                    }
                    else{
                        //normal mode
                        //if we can spawn a fish: pay price
                        if(Controller_Fish.instance.SpawnFish(guppyPrefab, new Vector3(0, 4, transform.position.z))){
                            Controller_Wallet.instance.SubMoney(fishPrice);
                            PrintTransaction();
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

    //if purchace was successfull, display visuals on screen/ears
    private void PrintTransaction(){

        //show price pop up
        Controller_PopUp.instance.CreateTextPopUp(string.Format("- {0}", fishPrice));
        //and sound pop up
        AudioManager.instance.PlaySoundFXClip(buySoundClip, transform, 1f, 1f);
    }


    public override void OnPointerEnter(PointerEventData eventData){
        
        string dispString = string.Format("Buy {0}\nCost: ${1}", fishType.ToString(), fishPrice);

        ToolTip.ShowToolTip(dispString);
    }

}
