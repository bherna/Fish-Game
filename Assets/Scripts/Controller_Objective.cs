using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller_Objective : MonoBehaviour
{

    //List of objective sprites, in order of purchase
    [SerializeField] List<Sprite> obj_sprite_list = new List<Sprite>();

    //list of objecive prices, in order of purchase
    [SerializeField] List<int> obj_price_list = new List<int>();

    //current objective index
    [SerializeField] int obj_index = 0;

    [SerializeField] int final_obj = 3;

    [SerializeField] GameObject buttonSprite;


    private void Start() {
        
        //update sprite
        buttonSprite.GetComponent<Image>().sprite = obj_sprite_list[obj_index];
    }

    //when button pushed to purchase
    public void OnPurchase(){

        //if enough money
        //buy
        if(Wallet.instance.IsAffordable(obj_price_list[obj_index])){

            //update money
            Wallet.instance.SubMoney(obj_price_list[obj_index]);

            //update index
            obj_index += 1;

            //is the final objective bought
            if(obj_index >= final_obj){
                SceneManager.LoadScene("MainMenu");
            }
            else{
                //update sprite
                buttonSprite.GetComponent<Image>().sprite = obj_sprite_list[obj_index];

            }
        }

        //else
        Debug.Log("Not enough money");
    }



}
