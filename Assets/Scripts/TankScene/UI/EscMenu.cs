
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    
    public void GoToMainMenu(){

        Controller_Objective.instance.GoToMainMenu();
    }

    public void Settings(){
        
    }

    public void ReturnToGame(){
        Controller_EscMenu.instance.CloseMainMenu();
    }
}
