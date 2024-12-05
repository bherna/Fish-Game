
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

    //both in seconds
    private int countDownTimer = 0;
    private const int timerStart = 4;

    private int comboLevel = 0;



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



    //the function every obj will reference
    //all they need to think about is adding to the combo
    public void AddToCombo(int amt = 1){

        //if we have no combo 
        if(comboLevel == 0){

            //start new combo
            countDownTimer = timerStart;
            comboLevel = amt;
            //update teext 
            text.text = comboLevel.ToString();
            //start countdown
            StartCoroutine(CountDown());
        }
        else if(comboLevel >= 1){

            //add to combo level
            comboLevel += amt;
            text.text = comboLevel.ToString();
            //reset countdown
            countDownTimer = timerStart;
        }
    }



    private IEnumerator CountDown(){

        while(countDownTimer > 0){

            //wait for 1 second to pass
            yield return new WaitForSeconds(1);

            //sub
            countDownTimer -= 1;

            Debug.Log("while loop pass done");
        }


        //else we lost the combo 
        //reset
        countDownTimer = 0;
        comboLevel = 0;
        text.text = "0";

    }
    

}
