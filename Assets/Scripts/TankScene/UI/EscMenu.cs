
using UnityEngine;
using UnityEngine.UI;

public class EscMenu : MonoBehaviour
{
    //reference gameobjects we want this button to run on
    //should be the children attached to this object (this onject is the esc - UI)
    [SerializeField] GameObject ExitLevel_ButtonObj;     //exit level button 
    [SerializeField] GameObject OutsideSpace_ButtonObj; //the empty space that will be used to close the menu easier, than button

    void OnEnable()
    {
        //Register Button Events
        ExitLevel_ButtonObj.GetComponent<Button>().onClick.AddListener(() => OnGoToMainMenu());
        OutsideSpace_ButtonObj.GetComponent<Button>().onClick.AddListener(() => OnCloseMenu());
        
    }

    void OnDisable()
    {
        //Un-Register Button Events
        ExitLevel_ButtonObj.GetComponent<Button>().onClick.RemoveAllListeners();
        OutsideSpace_ButtonObj.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void OnGoToMainMenu()
    {
        Controller_Objective.instance.GoToMainMenu();
    }

    public void Settings()
    {

    }

    private void OnCloseMenu()
    {
        Controller_EscMenu.instance.CloseMainMenu();
    }
    
    
}
