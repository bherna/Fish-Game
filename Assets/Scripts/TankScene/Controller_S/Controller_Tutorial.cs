
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class Controller_Tutorial : MonoBehaviour
{

    //references
    [SerializeField] GameObject shop_Container;
    [SerializeField] GameObject UI_Dialogue; //this is the actual GameObject, used in enabling and disabling ui 
    public bool tutorial_active {get; private set;} = true;
    public bool sell_tutorial_fish = false;

    //singleton this class
    public static Controller_Tutorial instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }



    void Start()
    {
        
        //we need to summon a tutorial reader, 
        //first make sure that there is a tutorial in this level
        if(LocalLevelVariables.ThereIsTutorial()){
            
            string name = string.Format("TutorialReader_{0}_{1}", LocalLevelVariables.GetTankWorld_String(), LocalLevelVariables.GetLevel_String());
            var tutReader = System.Type.GetType(name);
            transform.GetChild(0).gameObject.AddComponent(tutReader);
        }
        //if not then just ignore anything else in this script i guess...
        else{
            transform.GetChild(0).gameObject.AddComponent<TutorialReaderParent>();
        }
    }



    //used in tutorial reader class

    //used in disabling UI_Dialogue from being visable
    //true == set to visable
    //when in viewing from heirarchy , if i forget to reenable, so just do this for me
    public void SetUIActive(bool active){
        UI_Dialogue.SetActive(active);
    }
    public void DisableTutorial(){
        tutorial_active = false;
    }


    //used in disableing and enabling shop buttons
    public void SetShopItemActive(int index, bool active){
        shop_Container.transform.GetChild(index).GetComponent<Button>().interactable = active;
    }

    //get shop items length
    public int GetShopItemsCount(){
        return shop_Container.transform.childCount;
    }

    public UI_Dialogue GetUI_Dialogue(){
        return UI_Dialogue.GetComponent<UI_Dialogue>();
    }



}
