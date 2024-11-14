using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Controller_Objective : MonoBehaviour
{

    //ui image showing what egg sprite
    [SerializeField] Image ui_currEggSprite;

    //display current egg piece cost on screen
    [SerializeField] TextMeshProUGUI ui_displayCost;

    //once we beat this level (buy all three pieces) make this panel active
    [SerializeField] GameObject postGamePanel;

    //egg pieces sprite and prices associated
    private Sprite[] eggSprites;
    private int[] eggPrices;

    //current objective index
    private int obj_index = 0;

    //final objective index
    //there will always be 3 pieces to buy
    private int final_obj = 3;



    //singleton this class
    public static Controller_Objective instance {get; private set; }
    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }  





    private void Start() {

        //update our variables
        eggPrices = GameVariables.GetEggPiecesPrices();
        SetEggSprites();
        
        //update sprite
        ui_currEggSprite.sprite = eggSprites[obj_index];

        //display obj cost
        ui_displayCost.text = eggPrices[obj_index].ToString();

    
    }

    

    //when button pushed to purchase
    public void OnPurchase(){

        //if enough money
        //buy
        if(Controller_Wallet.instance.IsAffordable(eggPrices[obj_index])){

            //update money
            Controller_Wallet.instance.SubMoney(eggPrices[obj_index]);

            //update index
            obj_index += 1;

            //is the final objective bought
            if(obj_index >= final_obj){

                //deactivate button
                GetComponent<Button>().interactable = false;

                //*** level complete ***

                //new level should be unlocked and new pet?
                int[] worldLevel = GameVariables.GetlevelUnlock();
                LevelsAccess.UnlockLevel_Access(worldLevel[0], worldLevel[1]);

                //new pet
                var pet = GameVariables.GetPetUnlock();
                PetsAccess.UnlockPet_Access(pet);

                //save game
                SaveLoad.SaveGame();

                GameDone();
                
            }
            else{
                //update sprite
                ui_currEggSprite.sprite = eggSprites[obj_index];
                //update display obj cost
                ui_displayCost.text = eggPrices[obj_index].ToString();

            }
        }

        else{
            //Debug.Log("Not enough money");
        }
        
    }

    //run this when game is over
    public void GameDone(){

        //show stats
        //postgamepanel should let the player exit to main menu
        Controller_Timer.instance.StopTimer();
        postGamePanel.SetActive(true);
    }


    private void SetEggSprites(){

        string path = string.Format("EggsSprites/{0}/{0}_", GameVariables.GetPetUnlock_AsString());
        Debug.Log(path);

        Sprite[] newSprites = new Sprite[]{
            Resources.Load<Sprite>(path+"1"),
            Resources.Load<Sprite>(path+"2"),
            Resources.Load<Sprite>(path+"3")
        };

        eggSprites = newSprites;
    }

}
