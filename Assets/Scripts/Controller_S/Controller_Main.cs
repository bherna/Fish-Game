using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Main : MonoBehaviour
{

    //start money for current level
    [SerializeField] int startMoney;

    private IEnumerator coroutine;

    private int timeTillNextCheck = 5;

    //reference to self
    public static Controller_Main instance {get; private set; }
    //is the game currently paused
    public bool paused {get; private set;} = false;


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
        if(Input.GetKeyDown(KeyCode.Escape)){
            
            //pause game
            PauseLevel();

            //open escape menu

        }
    }

    
    //has the player lost function
    IEnumerator CheckGameState(int time){

        while(true){
            
            yield return new WaitForSeconds(time);

            //check if no money and no fish
            if(
                (Wallet.instance.current_money) == 0 &&
                (Controller_Fish.instance.GetFishCount() == 0)){
                    //end game
                    SceneManager.LoadScene("MainMenu");
                }
            print("yearafds");
        }
    }




    public void PauseLevel(){

        Time.timeScale = 0;
        AudioListener.pause = true;
        paused = true;
    }

    public void UnPauseLevel(){

        Time.timeScale = 1;
        AudioListener.pause = false;
        paused = false;
    }
}
