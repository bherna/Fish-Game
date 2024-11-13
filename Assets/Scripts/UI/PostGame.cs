using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostGame : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI showStats;

    private void Awake() {
        
        //once game is over we get activated
        //so

        //update our post game stats here:
        float finalTime = Controller_Timer.instance.GetFinalTime();
        int min = (int)math.floor(finalTime/60);
        int sec = (int)finalTime;
        string milli = Math.Floor(finalTime * 1000).ToString();
        milli = milli.Substring(milli.Length-3);

        showStats.text = string.Format("Final Time: \n{0}:{1}:{2}", min,sec,milli);
    }


    
    public void GoToMainMenu(){

        //return time and audio back to normal
        Time.timeScale = 1;
        AudioListener.pause = false;
        
        //return
        SceneManager.LoadScene("MainMenu");
    }
}
