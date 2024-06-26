using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Controller_Objective : MonoBehaviour
{

    //List of objective sprites, in order of purchase
    [SerializeField] List<Sprite> obj_sprite_list = new List<Sprite>();

    //list of objecive prices, in order of purchase
    [SerializeField] List<int> obj_price_list = new List<int>();

    //current objective index
    int obj_index = 0;

    //final objective index
    int final_obj = 3;

    //button gameobject 
    [SerializeField] GameObject buttonSprite;

    //display current obj cost on screen
    [SerializeField] TextMeshProUGUI ui_displayCost;


    private void Start() {

        try{

            //update the final objective index
            final_obj = obj_sprite_list.Count;

            //check if our sprite and price list are equal in lenght
            if(final_obj != obj_price_list.Count){
                Debug.Log("sprite list and price list is not the same.");
            }

        }catch(IndexOutOfRangeException e){

            Debug.Log("Error: "+e);
            Debug.Log("A list for our objectives is missing.");
        }
        
        //update sprite
        buttonSprite.GetComponent<Image>().sprite = obj_sprite_list[obj_index];

        //display obj cost
        ui_displayCost.text = obj_price_list[obj_index].ToString();

        

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
                //update display obj cost
                ui_displayCost.text = obj_price_list[obj_index].ToString();

            }
        }

        else{
            Debug.Log("Not enough money");
        }
        
    }



}
