
using System.Collections;
using TMPro;
using UnityEngine;


//this class is a singleton class since multiple objects are gonna reference it
//how this class works

    //we only function if some outside object calls us
    //once that happens, we can start doing things
        
        //addToCombo func: gives the player +1 to their combo (you can pass an arg to add more instead of default of 1)
        //from there we start to count down, 
            //if the player doesn't addToCOmbo again witin the timer, we lose the combo
                //from there we return to nothing
            //else we reset the countdown,
                //(an idea i have is making the countdown faster as we go higher in combo level)
public class UI_Combo : MonoBehaviour
{

    //text to edit
    private TextMeshProUGUI text;

    


    //singleton this class
    public static UI_Combo instance {get; private set; }
    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }   


    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = "0"; //for now
    }



    public void UpdateText(string newText)
    {
        text.text = newText;
    }
    

}
