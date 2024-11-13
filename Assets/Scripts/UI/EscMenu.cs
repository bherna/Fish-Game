using UnityEngine.SceneManagement;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    
    public void GoToMainMenu(){

        //return time and audio back to normal
        Time.timeScale = 1;
        AudioListener.pause = false;
        
        //return
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings(){
        
    }

    public void ReturnToGame(){
        Controller_EscMenu.instance.UnPauseLevel();
    }
}
