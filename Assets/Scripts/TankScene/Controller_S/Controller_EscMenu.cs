using UnityEngine;
using UnityEngine.UI;

public class Controller_EscMenu : MonoBehaviour
{

    //is the game currently paused
    public bool paused {get; private set;} = false;
    private bool escMenuOpen = false;

    //reference to ui tab holding store items
    [SerializeField] GameObject ui_tab;
    [SerializeField] GameObject ui_esc;





    //singleton this class
    public static Controller_EscMenu instance {get; private set; }
    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }   




    private void Update() {
        

        //escape - open menu
        if(Input.GetKeyUp(KeyCode.Escape) && !escMenuOpen){
            
            //pause game
            //open escape menu
            PauseLevel();
            escMenuOpen = true;


        }
        else if(Input.GetKeyUp(KeyCode.Escape) && escMenuOpen) {

            //unpause game
            //close escape menu
            UnPauseLevel();
            escMenuOpen = false;


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
