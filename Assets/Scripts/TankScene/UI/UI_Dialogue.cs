using System.Collections;
using UnityEngine;
using TMPro;




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

        return GetJsonScriptNumber("1");
        
    }


    //Used in opening the next json file that holds our next script for tutorial
    //if the json file exsists, then we have a script to run, return true
    //else return false
    //we also update our script in here, so we don't have to return it

    //For each tutorial, there should be a list of json files
    //The naming convention should be 'Tank_{0}-{1}'
    //{0} == the tank world we are using, 
    //{1} == this is the script index, we start at 1 and increment from there
    public bool GetJsonScriptNumber(string scriptNum){

        //Import in the json file that this tank_world-leve will use
        string filePath = "Tutorial/Tank_" + LocalLevelVariables.GetTankWorld()+"-"+scriptNum;

        //get the json file we want to read
        string targetFile = Resources.Load<TextAsset>(filePath).text;
        var wholeScript = JsonUtility.FromJson<WholeDialogue>(targetFile);
        script = wholeScript.script;
        lineCutOff = wholeScript.lineCutOff;
        pause = wholeScript.pause;

        //does this file (tank_world-level.json) exsist
        if(targetFile != null){

            //if yes, then we have a tutorial to run
            
            //first check for comments
            //while our current index has a '/' (for comments)
            while(lineIndex < script.Length && script[lineIndex].ToCharArray()[0] == '/'){
                //keep incrementing
                lineIndex++;
            }
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
            yield return new WaitForSeconds(textSpeed);
        }
    }


    //used for checking if a next line of dialogue/string exists, then starts that coroutine
    //if next line exists, return true
    //else false
    private bool NextLine(){

        //while we have next line
        //does this new index start with a '/' (for comments)
        while(lineIndex+1 < script.Length && script[lineIndex+1].ToCharArray()[0] == '/'){
            //keep incrementing
            lineIndex++;
        }

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

    //used in skipping the script
    //within the tutorial script, 
    public bool CanWeSkip(){
        return lineIndex >= lineCutOff;
    }


    //used for disabling or enabling in showing text ui box
    public void ToggleDialogueBox(bool toggle){

        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(toggle);
        }
    }


}
