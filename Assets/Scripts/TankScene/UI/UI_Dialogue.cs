using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



[System.Serializable]
public struct Single_Dialogue
{
    public string line;  //dialogue string
    public ExpectType expectType; //what dialogue string expects, to run
    
}


[System.Serializable]
public class WholeDialogue{
    public Single_Dialogue[] script; //same as json file
}


public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;

    private Single_Dialogue[] script;
    public ExpectType curr_expectType {get; private set; } 

    private int index;    
    private float textSpeed = 0.05f;


    

    //start method
    public void StartDialogue(){

        script = JsonUtility.FromJson<WholeDialogue>(LoadResourceTextfile()).script;

        //set string to empty/ index to 0
        textUI.text = string.Empty;
        index = 0;

        //set our next index expect type
        curr_expectType = script[index].expectType;

        //start dialogue
        StartCoroutine(TypeLine());
    }

    //read in the json file for this level
    private static string LoadResourceTextfile()
    {

        var curr_level = GameVariables.curr_level;
        //string filePath = "Tutorial/Tank_" + curr_level.Substring(curr_level.Length - 3);
        string filePath = "Tutorial/Tank_TutorialTemplate";

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
    }
    

    //This gets used outside of this class
    //When ever a 'click' is done this command is played
    //click helps move dialogue to next line / finish current dialogue line
    //returns nextline bool (true for more lines, false for we are done)
    //else return true , since we have another click togo
    public bool Click(){

        if(textUI.text == script[index].line){
            return NextLine();
        }
        else{
            StopAllCoroutines();
            textUI.text = script[index].line;
            return true;
        }
        
    }

    
    //slowly types out the current dialogue string we are index'd at
    private IEnumerator TypeLine(){
        //for each char 1 by 1
        foreach( char c in script[index].line.ToCharArray()){

            textUI.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    //used for checking if a next line of dialogue/string exists, then starts that coroutine
    //if next line exists, return true
    //else false
    private bool NextLine(){
        if (index < script.Length - 1){

            //increment index / reset ui text box / set next expect type
            index++;
            textUI.text = string.Empty;
            curr_expectType = script[index].expectType;
            //start typing line method and return
            StartCoroutine(TypeLine());
            return true;
        }
        else{

            return false;
        }
    }


    //used for disabling or enabling in showing text ui box
    public void ToggleDialogueBox(bool toggle){

        transform.GetChild(0).gameObject.SetActive(toggle);
        transform.GetChild(1).gameObject.SetActive(toggle);
    }

    //the enum WAIT expect type relies on this method
    //this method should expect a next index in the array
    public ExpectType GetNext_ExpectType(){
        return script[index+1].expectType;
    }

}
