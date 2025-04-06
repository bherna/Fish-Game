using System.Collections;
using UnityEngine;
using TMPro;
using System.Reflection;




[System.Serializable]
public class WholeDialogue{
    public string[] script; //same as json file

    public int lineCutOff; //if player does an event after this line number, we can accept it

    public bool pause; //should this current dialogue pause the game for the player, for ease of reading
}


public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;

    private string[] script; //the current json file, script we are going to display (section)
    private int lineIndex = 0;   //current line the dialogue is displaying
    private float textSpeed = 0.05f;                
    private int lineCutOff = 0;  // this is used in determining if the player can skip the rest of this current script
                                    //line index counts from 0, make sure to also not skip comment lines they count as well
    public bool pause {get; private set;}
    

    //start method
    //this is used in the controller_tutorial script
    //return true if we have a tutorial to run
    //else return false to skip running tutorial 
    public bool StartDialogue(){

        //reset values
        //set string to empty/ index to 0
        textUI.text = string.Empty;

        return GetJsonScriptNumber(1, 0);
        
    }


    //Used in opening the next json file that holds our next script for tutorial
    //if the json file exsists, then we have a script to run, return true
    //else return false
    //we also update our script in here, so we don't have to return it (just bool)

    //For each tutorial, there should be a list of json files
    //The naming convention should be 'Script_{1}-{2}'
    //{1} == the tutorial index, 
    //{2} == the alternative in this index, if none, set to 0, 0 == main script to run vs 1,2,3,... are alternatives
    public bool GetJsonScriptNumber(int scriptNum, int altNum){

        //Import in the json file that this tank_world-leve will use
        string filePath = string.Format("Json/TutorialScripts/{0}/Script_{1}-{2}", LocalLevelVariables.curr_level, scriptNum, altNum);

        //get the json file we want to read
        string targetFile = Resources.Load<TextAsset>(filePath).text;
        var wholeScript = JsonUtility.FromJson<WholeDialogue>(targetFile);
        script = wholeScript.script;
        lineCutOff = wholeScript.lineCutOff;
        pause = wholeScript.pause;

        //does this file (Script_{1}-{2}.json) exsist
        if(targetFile != null){

            //if yes, then we have a tutorial to run
            
            //first check for comments
            SkipComments(0);

            //now start dialogue
            StartCoroutine(TypeLine());
            return true;
        }
        else{
            return false;
        }
    }
    

    //This gets used in the controller_tutorial class
    //When ever a 'click' is done this command is played
    //click helps move dialogue to next line / finish current dialogue line
    //returns nextline bool (true for more lines, false for we are done)
    //else return true , since we have another click togo
    public bool Click(){

        //when our entire dialogue is in the text box, we can move on to next dialogue
        if(textUI.text == script[lineIndex]){
            return NextLine();
        }
        //our dialogue still hasn't finished printing, so insta finish it
        else{
            StopAllCoroutines();
            textUI.text = script[lineIndex];
            return true;
        }
        
    }

    
    //slowly types out the current dialogue string we are index'd at
    private IEnumerator TypeLine(){
        //for each char 1 by 1
        foreach( char c in script[lineIndex].ToCharArray()){

            textUI.text += c;
            yield return new WaitForSecondsRealtime(textSpeed); 
        }
    }


    //used for checking if a next line of dialogue/string exists, then starts that coroutine
    //if next line exists, return true
    //else false
    private bool NextLine(){

        SkipComments(1);

        if(lineIndex+1 < script.Length){
            //increment index / reset ui text box / set next expect type
            lineIndex++;
            textUI.text = string.Empty;

            //start typing line method and return
            StartCoroutine(TypeLine());
            return true;
        }
        else{
            //no more lines
            //reset values now
            lineIndex = 0;
            textUI.text = string.Empty;
            return false;
        }

    }


    //int param allows to check next index, instead of current line
    private void SkipComments(int plusN){

        //while we have next line
        //and current index is not empty (ie this "" )
        //does this new index start with a '/' (for comments)
        while(  lineIndex+plusN < script.Length &&
                script[lineIndex+plusN].Length > 0 &&
                script[lineIndex+plusN].ToCharArray()[0] == '/'){
            //keep incrementing
            lineIndex++;
        }
    }

    //used in skipping the script
    //within the tutorial script, 
    public bool CanWeSkip(){
        return lineIndex >= lineCutOff;
    }


    //used for disabling or enabling in showing text ui box
    public void ToggleDialogueBox(bool toggle){

        //for each part of the dialogue box (bg, outline, text ..)
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(toggle);
        }
    }


    //

}
