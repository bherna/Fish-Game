
using Assests.Inputs;
using TMPro;
using UnityEngine;





//this class is reference when we want to create a new pop up show up on the screen



public class Controller_PopUp : MonoBehaviour
{


    //
    [SerializeField] RectTransform canvasRecTrans; //the canvas-ui object
    [SerializeField] GameObject textPopUp; //actual prefab reference, not instantiated one
    [SerializeField] GameObject eggPopUp; //same   /\

    //this is the actual instanitated reference of eggpopup, whenever we need to move it
    private GameObject eggPopUp_ref;

    //single ton this class
    public static Controller_PopUp instance {get; private set; }

    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }




    //all we need is other  classes to reference the popup creation
    public void CreateTextPopUp(string displayText){

        //create a pop up
        var popup = Instantiate(textPopUp);
        popup.transform.SetParent(transform); //do this before setting position atleast (else we are placed at the bottom of entire stack, outside of ui canvas)
        popup.GetComponent<TextPopUp>().UpdateText(displayText);


        //im copying the tooltip class so just reference that if something breaks 
        var text = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>(); 
        text.ForceMeshUpdate(); //update mesh before changing bg dimensions

        //update the background dimensions to fit text
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(text.margin.x, text.margin.y*2);
        Vector2 fullSize = textSize+paddingSize;

        //update the tool tip position to be inside the screen view (so it doesnt type of screen)
        Vector2 anchoredPos = CustomVirtualCursor.MousePosition / canvasRecTrans.localScale.x;
        anchoredPos.x = Mathf.Clamp(anchoredPos.x, 0, canvasRecTrans.rect.width - fullSize.x);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, 0, canvasRecTrans.rect.height - fullSize.y);

        popup.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        
    }


    //same as this one, but controller objective will only use this (assuming)
    //used in creating a new ui egg that slowly centers on screen, given a starting position
    // (will always be the same size as the one in the tank)
    //how this is going to be done:
    //  - assuming the egg that called level complete is destroyed, we take its position
    //  - create a new UI version of the completed egg
    //  - start to move and zoom egg into desired position (middle of screen-ish)
    //  - remove saturation and pop it open (this will be place holder animation untill I learn to generalized egg hatches)
    //  - show new pet unlocked and name
    //  - continue ....
    public void StartEggHatch(Vector2 position){

        //create egg
        eggPopUp_ref = Instantiate(eggPopUp);
        eggPopUp_ref.transform.SetParent(transform); //do this before setting position atleast (else we are placed at the bottom of entire stack, outside of ui canvas)

        //update start position to match that of the in tank egg
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(position);   
        Vector2 startPos = new Vector2(viewportPosition.x * canvasRecTrans.sizeDelta.x, viewportPosition.y * canvasRecTrans.sizeDelta.y);
        Vector2 endPos = new Vector2(canvasRecTrans.rect.width/2, canvasRecTrans.rect.height/2 - 100);
        eggPopUp_ref.GetComponent<EggPopUp>().StartEndPoints(startPos, endPos);
        eggPopUp_ref.GetComponent<EggPopUp>().StartMoving();

    }



    //this is used to move pop ups away /hide from the ui dialogue text box area, 
    //else it will be hard to read what any of the text is saying
    public void DisplacePopUps(bool val){


        //for text popups, just hide them by setting alpha to 0
        foreach(Transform child in gameObject.transform)
        {
            var script = child.gameObject.GetComponent<TextPopUp>();

            if(script != null){
                script.SetTextAlpha(val);
            }
        }


        //for egg pop up, we want to displace its position
        //first check if we have an eggpopup
        if(eggPopUp_ref == null){return;}
        //now displace
        if(val){

            //val == true means we want to return 
            //place at dead endpos again
            Vector2 endPos = new Vector2(canvasRecTrans.rect.width/2, canvasRecTrans.rect.height/2 - 100);
            eggPopUp_ref.transform.position = endPos;

        }
        else{
            //move up higher
            Vector2 endPos = new Vector2(canvasRecTrans.rect.width/2, canvasRecTrans.rect.height/2 + 200);
            eggPopUp_ref.transform.position = endPos;
        }

    }
}
