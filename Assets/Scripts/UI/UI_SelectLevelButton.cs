
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectLevelButton : MonoBehaviour
{

    //reference to which tank scene we are going to use
    [SerializeField] string tankScene;
    //reference to the name of the level (ie: level_1-1 level_1-2 ...)
    [SerializeField] string level;

    void OnEnable()
    {
        //Register Button Events
        GetComponent<Button>().onClick.AddListener(() => MainMenu.instance.GoToPetsPanel(tankScene, level));
    }

    void OnDisable()
    {
        //Un-Register Button Events
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
    

    
}
