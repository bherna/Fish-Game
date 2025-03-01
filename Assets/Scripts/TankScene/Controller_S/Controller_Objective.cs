using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.PlayerLoop;


public class Controller_Objective : Shopables_ParentClass, IPointerEnterHandler, IPointerExitHandler
{

    //once we beat this level (buy all three pieces) make this panel active
    [SerializeField] GameObject postGamePanel;

    //the three sound effects when buying
    [SerializeField] AudioClip[] eggPieces_audioClips;

    //egg piece gamobject to spawn into tank
    [SerializeField] GameObject eggPiece;


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
        eggPrices = LocalLevelVariables.GetEggPiecesPrices();
        SetEggSprites();
        
        //update sprite
        currSprite.sprite = eggSprites[obj_index];

    
    }

    

    

    //when button pushed to purchase
    public override void OnPurchase(){

        //if enough money
        //buy
        if(Controller_Wallet.instance.IsAffordable(eggPrices[obj_index])){

            //update money
            Controller_Wallet.instance.SubMoney(eggPrices[obj_index]);


            //show we bought, with a pop up
            Controller_PopUp.instance.CreatePopUp(string.Format("- {0}", eggPrices[obj_index]));

            //show we bought, with sound effect as well
            AudioManager.instance.PlaySoundFXClip(eggPieces_audioClips[obj_index], transform, 1f, 1f);

            //spawn egg piece
            var newEggPiece = Instantiate(eggPiece, Vector2.zero, Quaternion.identity);
            newEggPiece.GetComponent<EggPiece>().index.Add(obj_index);
            newEggPiece.GetComponent<EggPiece>().newSprite = eggSprites[obj_index];


            //----- ---- update index (LAST, else we break) ---- ---- //
            obj_index += 1;

            //update tooltip if needed
            ToolTip.ShowToolTip(DisplayText());

            //is the final objective bought
            if(obj_index >= final_obj){

                //deactivate button
                GetComponent<Button>().interactable = false;

                //*** level complete ***

                //new level should be unlocked and new pet?
                int[] worldLevel = LocalLevelVariables.GetlevelUnlock();
                LevelsAccess.UnlockLevel_Access(worldLevel[0], worldLevel[1]);

                //new pet
                var pet = LocalLevelVariables.GetPetUnlock();
                PetsAccess.UnlockPet_Access(pet);

                //save game
                SaveLoad.SaveGame();

                GameDone();
                
            }
            else{
                //update sprite
                currSprite.sprite = eggSprites[obj_index];

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

        string path = string.Format("EggsSprites/{0}/{0}_", LocalLevelVariables.GetPetUnlock_AsString());
        //Debug.Log(path);

        Sprite[] newSprites = new Sprite[]{
            Resources.Load<Sprite>(path+"1"),
            Resources.Load<Sprite>(path+"2"),
            Resources.Load<Sprite>(path+"3")
        };

        eggSprites = newSprites;
    }


    private string DisplayText(){

        //if we bought the last egg peice, dont show
        if(obj_index >= 3){
            return string.Format("No more peices to buy");
        }
        else{
            return string.Format("Buy: Egg Piece {0} \nCost: {1}", obj_index, eggPrices[obj_index].ToString());
        }
        
    }


    public override void OnPointerEnter(PointerEventData eventData){

        ToolTip.ShowToolTip(DisplayText());
    }

    

}
