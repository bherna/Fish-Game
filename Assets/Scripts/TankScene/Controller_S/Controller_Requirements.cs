using TMPro;
using UnityEngine;

public class Controller_Requirements : MonoBehaviour
{



    //Each built egg will need to follow some ritual/ requirements need to be cocmpleted to  finally hatch the egg
    // Each Egg will have its own unique way of hatching, since each pet is unique from each one of anohter

    //to start requiremtns, the egg peices need to be all bought and assembled,
    //once that is done, the egg will send an event to this class to start running this code
    //this script's only goal is to be a on/off switch to the acutal requirments class
    public void StartDisplaying(){
        Requirements_Panel.SetActive(true);
        GetComponent<PetReq_ParentClass>().StartReqs(); //toggle on funcc
    }


    //also we need to have a reference  to the ui element that holds the text for displaying requirements
    [SerializeField] TextMeshProUGUI showReqs;
    //the entire panel holding requirements ui, used in hiding
    [SerializeField] GameObject Requirements_Panel;
    [SerializeField] RectTransform backgroundRecTrans; //same requirements panel just the acutal rect trans

    public void UpdateReqs(string newText){
        showReqs.text = newText;
        showReqs.ForceMeshUpdate(); //update mesh before changing bg dimensions

        //update the background dimensions to fit text
        Vector2 textSize = showReqs.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(showReqs.margin.x+50, showReqs.margin.y*2+50);

        backgroundRecTrans.sizeDelta = textSize + paddingSize;
    }



    //since requiremtns  are unique per egg, we need a way to differ between each egg
    //there will be the PetReq_Parent.class that will hold all requirements while
    //  PetReq_{petname}.class  will  hold what requiremtns to use for current pet egg
    // this is similar to how tutorial class is built
    //i could just use a  giant switch case  statement, but that gets messy fast, so im doing this

    void Start()
    {

        string name = string.Format("PetReq_{0}", LocalLevelVariables.GetUnlockPet_Name());
        var tutReader = System.Type.GetType(name);

        if(tutReader != null){

            gameObject.AddComponent(tutReader);
        }
        else{
            Debug.Log("noe pet Requirements script avilable");
            gameObject.AddComponent<PetReq_ParentClass>();
        }

    }




    //singleton this class
    public static Controller_Requirements instance {get; private set; }
    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }  



    


}
