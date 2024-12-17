
using TMPro;
using UnityEngine;





//this class is reference when we want to create a new pop up show up on the screen



public class Controller_PopUp : MonoBehaviour
{


    //
    [SerializeField] RectTransform canvasRecTrans; //the canvas-ui object
    [SerializeField] GameObject textPopUp;

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
    public void CreatePopUp(string displayText){

        //create a pop up
        var popup = Instantiate(textPopUp);
        popup.transform.SetParent(transform); //do this before setting position atleast
        popup.GetComponent<TextPopUp>().UpdateText(displayText);


        //im copying the tooltip class so just reference that if something breaks 
        var text = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>(); 
        text.ForceMeshUpdate(); //update mesh before changing bg dimensions

        //update the background dimensions to fit text
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(text.margin.x, text.margin.y*2);
        Vector2 fullSize = textSize+paddingSize;

        //update the tool tip position to be inside the screen view (so it doesnt type of screen)
        Vector2 anchoredPos = Input.mousePosition / canvasRecTrans.localScale.x;
        anchoredPos.x = Mathf.Clamp(anchoredPos.x, 0, canvasRecTrans.rect.width - fullSize.x);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, 0, canvasRecTrans.rect.height - fullSize.y);

        popup.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        
        //Input.mousePosition + Vector3.right;
    }
}
