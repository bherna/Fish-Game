
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_EscMenu : MonoBehaviour
{
    //reference gameobjects we want this button to run on
    //should be the children attached to this object (this onject is the esc - UI)
    [SerializeField] GameObject ExitLevel_ButtonObj;     //exit level button 
    [SerializeField] GameObject OutsideSpace_ButtonObj; //the empty space that will be used to close the menu easier, than button
    [SerializeField] GameObject ApplySettings_ButtonObj; //used in updating all settings changed in esc menu


    //a list to hold all the settings scripts in one spot, so i dont overcrowd the inpector
    [SerializeField] List<GameObject> SettingsScripts = new List<GameObject>();  //list is filled in inpector for clarity


    void OnEnable()
    {
        //Register Button Events
        ExitLevel_ButtonObj.GetComponent<Button>().onClick.AddListener(() => OnGoToMainMenu());
        OutsideSpace_ButtonObj.GetComponent<Button>().onClick.AddListener(() => OnCloseMenu());
        ApplySettings_ButtonObj.GetComponent<Button>().onClick.AddListener(() => OnApplySettings());
        
    }

    void OnDisable()
    {
        //Un-Register Button Events
        ExitLevel_ButtonObj.GetComponent<Button>().onClick.RemoveAllListeners();
        OutsideSpace_ButtonObj.GetComponent<Button>().onClick.RemoveAllListeners();
        ApplySettings_ButtonObj.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void OnGoToMainMenu()
    {
        Controller_EscMenu.instance.GoToMainMenu();
    }

    private void OnCloseMenu()
    {
        Controller_EscMenu.instance.CloseMainMenu();
    }
    

    private void OnApplySettings()
    {
        //Debug.Log(string.Format("click, length: {0}", SettingsScripts.Count));
        foreach(GameObject obj in SettingsScripts)
        {
            var script = obj.GetComponent<Settings_Apply_Parent>();
            script.ApplySettings();
        }
    }
    
}
