
using TMPro;
using UnityEngine;


public class ToolTip : MonoBehaviour
{

    public static ToolTip instance {get; private set;}

    [SerializeField] private RectTransform canvasRecTrans; //used to get camera scalling (canvas-ui)
    private RectTransform backgroundRecTrans; //background image's vector
    private TextMeshProUGUI text; //updating text on screen
    private RectTransform rectTrans; //used in updating our position on canvas
    private System.Func<string> toolTipFunc; 


    // Start is called before the first frame update
    void Awake()
    {
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }


        backgroundRecTrans = transform.GetChild(0).GetComponent<RectTransform>();
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>(); 
        rectTrans = transform.GetComponent<RectTransform>();

        Hide(); //hide tool tip
    }

    
    private void SetText(string newText){

        //set text
        text.SetText(newText);
        text.ForceMeshUpdate(); //update mesh before changing bg dimensions

        //update the background dimensions to fit text
        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(text.margin.x, text.margin.y*2);

        backgroundRecTrans.sizeDelta = textSize + paddingSize;
    }


    private void Update(){

        //update text
        SetText(toolTipFunc());


        //update our tool tip position
        Vector2 anchoredPos = Input.mousePosition / canvasRecTrans.localScale.x;

        anchoredPos.x = Mathf.Clamp(anchoredPos.x, 0, canvasRecTrans.rect.width - backgroundRecTrans.rect.width);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, 0, canvasRecTrans.rect.height - backgroundRecTrans.rect.height);

        rectTrans.anchoredPosition = anchoredPos;
    }



    //show functions
    //first show just given a string, we delegate our string argument
    private void Show(string toolTipText){
        Show(() => toolTipText);
    }
    //second show is used to take in a string return type function
    private void Show(System.Func<string> toolTipFunc){
        this.toolTipFunc = toolTipFunc;
        gameObject.SetActive(true);
        SetText(toolTipFunc());
    }
    //third is the public version of second
    public static void ShowToolTip(System.Func<string> toolTipStringFunc){
        instance.Show(toolTipStringFunc);
    }
    public static void ShowToolTip(string toolTipString){
        instance.Show(toolTipString);
    }

    //hide functions
    private void Hide(){
        gameObject.SetActive(false);
    }
    public static void HideToolTip(){
        instance.Hide();
    }
}
