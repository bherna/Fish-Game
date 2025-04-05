using System;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGame : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI showStats;

    private void Awake() {
        
        //once game is over we get activated

        //do we need to do tutorial stuff first?
        if(TutorialReaderParent.instance.PostGameUI()){
            //this should return true on tutorial 1-1
            //now re-disable post game
            gameObject.SetActive(false);
        }

        //update our post game stats here:
        float finalTime = Controller_Timer.instance.GetFinalTime();

        //get minutes (minutes is every 60 seconds)
        int min = (int)math.floor(finalTime/60);    
        //get seconds, seconds should be between (0 - 59)
        int sec = (int)finalTime % 60;     
        //get milli seconds, the decimal part of our total time    (multiply by 1000 to move 3 places, mod by 1000 to remove seconds)         
        string milli = (Math.Floor(finalTime * 1000) % 1000).ToString();



        //final string to show
        showStats.text = string.Format("Final Time: \n{0}:{1}:{2}\n\n <b>You Unlocked {3} !</b>", min,sec,milli, LocalLevelVariables.GetUnlockPet_Name());
        
    }


    
    public void GoToMainMenu(){

        //return time and audio back to normal
        Time.timeScale = 1;
        AudioListener.pause = false;
        
        //return
        SceneManager.LoadScene("MainMenu");
    }
}
