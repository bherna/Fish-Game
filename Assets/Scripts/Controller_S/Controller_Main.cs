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

    private void Start() {
        
        Wallet.instance.AddMoney(startMoney);

        //check game state, if the player lost
        coroutine = CheckGameState(timeTillNextCheck);
        StartCoroutine(coroutine);

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
}
