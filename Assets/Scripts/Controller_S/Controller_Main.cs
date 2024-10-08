using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller_Main : MonoBehaviour
{

    //start money for current level
    [SerializeField] int startMoney;

    private IEnumerator coroutine;

    private int timeTillNextCheck = 5;

    
    //is the game currently paused
    public bool paused {get; private set;} = false;
    private bool escMenuOpen = false;

    //reference to ui tab holding store items
    [SerializeField] GameObject ui_tab;
    [SerializeField] GameObject ui_esc;





    //singleton this class
    public static Controller_Main instance {get; private set; }
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
        
        Wallet.instance.AddMoney(startMoney);

        //check game state, if the player lost
        coroutine = CheckGameState(timeTillNextCheck);
        StartCoroutine(coroutine);

    }


    private void Update() {
        

        //escape - open menu
        if(Input.GetKeyUp(KeyCode.Escape) && !escMenuOpen){
            
            //pause game
            PauseLevel();
            escMenuOpen = true;

            //open escape menu

        }
        else if(Input.GetKeyUp(KeyCode.Escape) && escMenuOpen) {

            //unpause game
            UnPauseLevel();
            escMenuOpen = false;

            //close escape menu

        }
    }

    
    //checks to see if the player lost 
    IEnumerator CheckGameState(int time){

        while(true){
            
            yield return new WaitForSeconds(time);

            //check if no money and no fish
            if(
                Wallet.instance.current_money == 0 &&
                Controller_Fish.instance.GetFishCount() == 0
                ){
                    //end game
                    SceneManager.LoadScene("MainMenu");
                }

        }
    }




    public void PauseLevel(){

        //pause all time references (physics, time)
        Time.timeScale = 0;
        //pause audio listeners
        AudioListener.pause = true;
        //paused boolean (other scripts reference this for user onclick event)
        paused = true;

        //disable ui buttons (so player can't purchase)
        foreach(var btn in ui_tab.GetComponentsInChildren<Button>(true)){
            btn.interactable = false;
        }

        //enable esc ui
        ui_esc.SetActive(true);
    }

    public void UnPauseLevel(){

        Time.timeScale = 1;
        AudioListener.pause = false;
        paused = false;

        foreach(var btn in ui_tab.GetComponentsInChildren<Button>(true)){
            btn.interactable = true;
        }

        //disable esc ui
        ui_esc.SetActive(false);
    }



    
}
